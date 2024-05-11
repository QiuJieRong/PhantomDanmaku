using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityEditor;

namespace PhantomDanmaku
{
    public class UIFormPlaceholder : MonoBehaviour
    {
        [ValueDropdown("ValuesGetter", IsUniqueList = true)]
        public List<Component> FieldInChildUIForm;

        [Button("生成代码")]
        public void Generate()
        {
            #region 生成后不可修改的代码

            var members = new List<MemberDeclarationSyntax>();
            //为这个类添加勾选的组件字段
            foreach (var component in FieldInChildUIForm)
            {
                members.Add(
                    SyntaxFactory.FieldDeclaration(
                        SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName($"{component.GetType().Name}"))
                            .AddVariables(
                                SyntaxFactory.VariableDeclarator($"m_{component.name}{component.GetType().Name}"))
                    ).AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword))
                );
            }

            // 创建一个基本的InstallField函数的结构
            MethodDeclarationSyntax method = SyntaxFactory
                .MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                    "InstallField")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.OverrideKeyword))
                .WithBody(SyntaxFactory.Block());

            // 在函数中添加base.InstallField();
            method = method.AddBodyStatements(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.BaseExpression(),
                            SyntaxFactory.IdentifierName("InstallField")))));

            List<StatementSyntax> assignmentStatements = new List<StatementSyntax>();

            //字段赋值语句
            foreach (var component in FieldInChildUIForm)
            {
                var path = component.GetRelativePath(transform, component.transform);
                assignmentStatements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName($"m_{component.name}{component.GetType().Name}"),
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("transform"),
                                            SyntaxFactory.IdentifierName("Find")),
                                        SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(
                                            SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal(path)))))),
                                    SyntaxFactory.IdentifierName($"GetComponent<{component.GetType().Name}>")),
                                SyntaxFactory.ArgumentList()))));
            }

            // 将所有的赋值语句添加到函数体中
            method = method.AddBodyStatements(assignmentStatements.ToArray());

            members.Add(method);

            // 构建一个类声明
            var classDeclaration = SyntaxFactory.ClassDeclaration(gameObject.name)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword))
                .AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName("UIFormBase")))
                .AddMembers(members.ToArray());


            // 构建命名空间
            var namespaceDeclaration = SyntaxFactory
                .NamespaceDeclaration(SyntaxFactory.ParseName("PhantomDanmaku.Runtime.UI"))
                .AddMembers(classDeclaration);

            var usingDirectiveList = new List<UsingDirectiveSyntax>();
            foreach (var component in FieldInChildUIForm)
            {
                var type = component.GetType();
                if (!usingDirectiveList.Exists(usingDirective => usingDirective.Name.ToFullString() == type.Namespace))
                {
                    usingDirectiveList.Add(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName($"{type.Namespace}")));
                }
            }
            
            // 将using语句添加到语法树
            var syntaxTree = SyntaxFactory.SyntaxTree(namespaceDeclaration.AddUsings(usingDirectiveList.ToArray()));

            // 将语法树转换为字符串并进行格式化
            var code = syntaxTree.GetRoot().NormalizeWhitespace().ToFullString();

            // 将字符串写入文件
            File.WriteAllText($"Assets/PhantomDanmaku/Scripts/UI/UIForm/{gameObject.name}.cs", code);

            #endregion

            #region 如果文件已经存在则不生产的代码

            if (!File.Exists($"Assets/PhantomDanmaku/Scripts/UI/UIForm/{gameObject.name}Logic.cs"))
            {
                
                // 构建一个类声明
                var logicClassDeclaration = SyntaxFactory.ClassDeclaration(gameObject.name)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));


                // 构建命名空间
                var logicNamespaceDeclaration = SyntaxFactory
                    .NamespaceDeclaration(SyntaxFactory.ParseName("PhantomDanmaku.Runtime.UI"))
                    .AddMembers(logicClassDeclaration);

                // 将using语句添加到语法树
                var logicSyntaxTree = SyntaxFactory.SyntaxTree(logicNamespaceDeclaration);

                // 将语法树转换为字符串并进行格式化
                var logicCode = logicSyntaxTree.GetRoot().NormalizeWhitespace().ToFullString();

                // 将字符串写入文件
                File.WriteAllText($"Assets/PhantomDanmaku/Scripts/UI/UIForm/{gameObject.name}Logic.cs", logicCode);
            }

            #endregion
            
            AssetDatabase.Refresh();
        }

        public IEnumerable ValuesGetter()
        {
            var values = new List<ValueDropdownItem>();
            var components = gameObject.GetComponentsInChildren(typeof(Component));
            foreach (var component in components)
            {
                var path = component.GetRelativePath(transform, component.transform) +
                           component.ToString().Replace(" ", "");
                values.Add(new ValueDropdownItem(path, component));
            }

            return values;
        }
    }
}