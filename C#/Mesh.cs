//本文件由工具生成，请勿直接修改
using System;
using System.Collections.Generic;
namespace Config
{

	[Serializable]
	public partial class Mesh : Sdp.IStruct
	{
		public List<Vec3> Vesters = List<Vec3>();
		public List<int> Triangle = List<int>();
		public Transform Trans = Transform();

		public void Visit(Sdp.ISdp sdp)		{
			sdp.Visit(1, "Vesters", false, ref Vesters);
			sdp.Visit(2, "Triangle", false, ref Triangle);
			sdp.Visit(3, "Trans", false, ref Trans);
		}
	}
}
