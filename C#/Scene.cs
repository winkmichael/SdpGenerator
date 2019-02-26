//本文件由工具生成，请勿直接修改
using System;
using System.Collections.Generic;
namespace Config
{

	[Serializable]
	public partial class Scene : Sdp.IStruct
	{
		public List<Mesh> Meshs = List<Mesh>();
		public List<Box> Boxs = List<Box>();
		public Dictionary<int,int> MapTest = Dictionary<int,int>();
		public Dictionary<ObjType,int> MapEnum = Dictionary<ObjType,int>();

		public void Visit(Sdp.ISdp sdp)		{
			sdp.Visit(1, "Meshs", false, ref Meshs);
			sdp.Visit(2, "Boxs", false, ref Boxs);
			sdp.Visit(3, "MapTest", false, ref MapTest);
			sdp.Visit(4, "MapEnum", false, ref MapEnum);
		}
	}
}
