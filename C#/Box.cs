//本文件由工具生成，请勿直接修改
using System;
using System.Collections.Generic;
namespace Config
{

	[Serializable]
	public partial class Box : Sdp.IStruct
	{
		public Vec3 Size = Vec3();

		public void Visit(Sdp.ISdp sdp)		{
			sdp.Visit(1, "Size", false, ref Size);
		}
	}
}
