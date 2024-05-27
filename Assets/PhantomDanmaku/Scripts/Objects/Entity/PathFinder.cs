using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PhantomDanmaku.Runtime
{
    public class PathFinder
    {
        private List<PathNode> m_OpenList;

        private List<PathNode> m_CloseList;

        /// <summary>
        /// 放置墙的TileMap
        /// </summary>
        private Tilemap m_WallTilemap;

        /// <summary>
        /// 起点
        /// </summary>
        private Vector2Int m_StartPoint;
        
        /// <summary>
        /// 终点
        /// </summary>
        private Vector2Int m_Destination;

        private static Vector2Int[] s_DirArray = {Vector2Int.up, Vector2Int.right,Vector2Int.down, Vector2Int.left};

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(Tilemap tilemap,Vector2Int startPoint, Vector2Int destination)
        {
            m_WallTilemap = tilemap;

            m_StartPoint = startPoint;

            m_Destination = destination;
            
            m_OpenList = new List<PathNode>();

            m_CloseList = new List<PathNode>();
        }

        /// <summary>
        /// 获得路径列表
        /// </summary>
        /// <returns></returns>
        public async UniTask<Stack<Vector2Int>> GetPathList()
        {
            var dis = Vector2.Distance(m_StartPoint, m_Destination);
            var startPathNode = new PathNode(m_StartPoint, null, 0, dis);

            m_OpenList.Add(startPathNode);

            var interval = 1f / Application.targetFrameRate;
            var startTime = Time.realtimeSinceStartup;
            
            while (m_OpenList.Count > 0)
            {
                if (Time.realtimeSinceStartup - startTime > interval)
                    await UniTask.DelayFrame(1);
                //获取代价最低的节点
                var minF = float.MaxValue;
                PathNode minPathNode = null;
                foreach (var pathNode in m_OpenList)
                {
                    if (pathNode.F < minF)
                    {
                        minF = pathNode.F;
                        minPathNode = pathNode;
                    }
                }
                //将该节点加入闭列表
                m_OpenList.Remove(minPathNode);
                m_CloseList.Add(minPathNode);

                if (minPathNode != null)
                {
                    //将该节点周围的节点加入开列表，并计算代价
                    foreach (var dir in s_DirArray)
                    {
                        var neighbor = minPathNode.Pos + dir;

                        if (m_OpenList.Find(node => node.Pos == neighbor) != null)
                        {
                            var pathNode = m_OpenList.Find(node => node.Pos == neighbor);
                            //计算此时的代价
                            var g = minPathNode.G + 1;
                            var h = Vector2Int.Distance(neighbor, m_Destination);
                            var f = g + h;
                            //判断是不是更小了
                            if (f < pathNode.F)
                            {
                                //如果更小了则更新
                                pathNode.Update(minPathNode,g,h);
                            }
                        }
                        //如果该地方不是墙壁,并且不在闭列表里
                        else if (m_WallTilemap.GetTile(new Vector3Int(neighbor.x, neighbor.y, 0)) == null &&
                            m_CloseList.Find(node => node.Pos == neighbor) == null)
                        {
                            //计算代价
                            var g = minPathNode.G + 1;
                            var h = Vector2Int.Distance(neighbor, m_Destination);

                            var pathNode = new PathNode(neighbor, minPathNode, g, h);
                            
                            //加入开列表
                            m_OpenList.Add(pathNode);
                            
                            //如果该位置是终点
                            if (neighbor == m_Destination)
                            {
                                var result = new Stack<Vector2Int>();
                                while (pathNode.PrePathNode != null)
                                {
                                    result.Push(pathNode.Pos);
                                    pathNode = pathNode.PrePathNode;
                                }
                                return result;
                            }
                        }
                    }
                    
                }
            }
            return null;
        }
    }

    public class PathNode
    {

        /// <summary>
        /// 位置
        /// </summary>
        private Vector2Int m_Pos;
        public Vector2Int Pos => m_Pos;

        public PathNode PrePathNode;
        
        /// <summary>
        /// 代价
        /// </summary>
        private float m_G;
        public float G => m_G;

        private float m_H;
        public float H => m_H;

        public float F => m_G + m_H;

        public PathNode(Vector2Int pos, PathNode prePathNode, float g, float h)
        {
            m_Pos = pos;
            PrePathNode = prePathNode;
            m_G = g;
            m_H = h;
        }

        public void Update(PathNode prePathNode, float g, float h)
        {
            PrePathNode = prePathNode;
            m_G = g;
            m_H = h;
        }
    }
}