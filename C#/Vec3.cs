//本文件由工具生成，请勿直接修改
using System;
using System.Collections.Generic;
namespace Config
{

	[Serializable]
	public partial class Vec3 : Sdp.IStruct
	{
		public float X1;
		public float Y;
		public float Z;

		public void Visit(Sdp.ISdp sdp)		{
			sdp.Visit(1, "X1", false, ref X1);
			sdp.Visit(2, "Y", false, ref Y);
			sdp.Visit(3, "Z", false, ref Z);
		}
	}
}
