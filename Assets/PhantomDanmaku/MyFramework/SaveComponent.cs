using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MyFramework.Runtime
{
    public class SaveComponent : GameFrameworkComponent
    {
        //保存路径
        public string SavePath => Application.persistentDataPath + "/Save";

        /// <summary>
        /// 存档主版本
        /// </summary>
        private int m_SaveMainVersion;

        /// <summary>
        /// 存档子版本
        /// </summary>
        private int m_SaveSubVersion;

        private void Start()
        {
            m_SaveMainVersion = 0;
            m_SaveSubVersion = 0;
        }

        /// <summary>
        /// 二进制同步保存
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="saveObject"></param>
        public void BinarySave<T>(string fileName, T saveObject)
        {
            var path = SavePath + $"/{fileName}";
            
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
            
            using (var fileStream = File.Create(path))
            {
                var binaryWriter = new BinaryWriter(fileStream);
                //写入存档版本号
                binaryWriter.Write(m_SaveMainVersion);
                binaryWriter.Write(m_SaveSubVersion);
                
                var memoryStream = new MemoryStream();
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream,saveObject);
                
                var length = memoryStream.Length;
                memoryStream.Position = 0;
                
                //写入存档长度
                binaryWriter.Write(length);

                //写入存档
                fileStream.Write(memoryStream.GetBuffer());
            }
        }
        
        /// <summary>
        /// 二进制异步保存
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="saveObject"></param>
        public async UniTask BinarySaveAsync<T>(string fileName, T saveObject)
        {
            var path = SavePath + $"/{fileName}";
            
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
            
            using (var fileStream = File.Create(path))
            {
                var binaryWriter = new BinaryWriter(fileStream);
                //写入存档版本号
                binaryWriter.Write(m_SaveMainVersion);
                binaryWriter.Write(m_SaveSubVersion);
                
                var memoryStream = new MemoryStream();
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream,saveObject);
                
                var length = memoryStream.Length;
                memoryStream.Position = 0;
                
                //写入存档长度
                binaryWriter.Write(length);

                //写入存档
                await fileStream.WriteAsync(memoryStream.GetBuffer());
            }
        }

        /// <summary>
        /// 二进制加载
        /// </summary>
        /// <param name="fileName"></param>
        public T BinaryLoad<T>(string fileName,Action updateAction = null)
        {
            var path = SavePath + $"/{fileName}";

            if (!File.Exists(path))
            {
                Debug.LogError("文件不存在");
                return default;
            }
            
            using (var fileStream = File.Open(path, FileMode.Open))
            {
                var binaryReader = new BinaryReader(fileStream);
                //读取版本信息
                var saveMainVersion = binaryReader.ReadInt32();
                var saveSubVersion = binaryReader.ReadInt32();
                if (saveMainVersion < m_SaveMainVersion)
                {
                    //存档主版本低于当前主版本，需要重新创建存档
                }
                else if (saveSubVersion < m_SaveSubVersion)
                {
                    //存档子版本低于当前子版本，需要进行存档更新
                    updateAction?.Invoke();
                }
                
                var saveLength = binaryReader.ReadInt64();

                Debug.Log($"存档长度:{saveLength}");
                var binaryFormatter = new BinaryFormatter();

                return (T)binaryFormatter.Deserialize(fileStream);
            }
        }
    }
}