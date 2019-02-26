//本文件由工具生成，请勿直接修改
#pragma once
#include <string>
#include <vector>
#include <map>
#include <algorithm>
namespace Config
{
	struct Vec3
	{
		public float X1 = 0;
		public float Y = 0;
		public float Z = 0;

		Vec3() = default;
		const char *getName() const { return "Vec3"; } 
		template <typename T>
		void visit(T &t, bool )
		{
			t.visit(1, false, "X1", X1);
			t.visit(2, false, "Y", Y);
			t.visit(3, false, "Z", Z);
		}
		template <typename T>
		void visit(T &t, bool ) const
		{
			t.visit(1, false, "X1", X1);
			t.visit(2, false, "Y", Y);
			t.visit(3, false, "Z", Z);
		}
		void swap(Vec3 &b)
		{
			std::swap(X1, b.X1);
			std::swap(Y, b.Y);
			std::swap(Z, b.Z);
		bool operator== (const Vec3 &rhs) const
		{
			return X1 == rhs.X1
				&& Y == rhs.Y
				&& Z == rhs.Z;
		}
		bool operator!= (const Vec3 &rhs) const
		{
			return !((*this) == rhs);
		}
	}
}
namespace std
{
	inline void swap(Config::Vec3 &a, Config::Vec3 &b) { a.swap(b); }
}
