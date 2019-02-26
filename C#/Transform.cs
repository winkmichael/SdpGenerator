//本文件由工具生成，请勿直接修改
using System;
using System.Collections.Generic;
namespace Config
{

	[Serializable]
	public partial class Transform : Sdp.IStruct
	{
		public Vec3 Pos = Vec3();
		public Vec3 Forward = Vec3();
		public Vec3 Sacle = Vec3();
		public ObjType Type;

		public void Visit(Sdp.ISdp sdp)		{
			sdp.Visit(1, "Pos", false, ref Pos);
			sdp.Visit(2, "Forward", false, ref Forward);
			sdp.Visit(3, "Sacle", false, ref Sacle);
			sdp.VisitEunm(4, "Type", false, ref Type);
		}
	}
}
