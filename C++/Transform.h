//本文件由工具生成，请勿直接修改
#pragma once
#include <string>
#include <vector>
#include <map>
#include <algorithm>
#include "Vec3.h"
#include "ObjType.h"
namespace Config
{
	struct Transform
	{
		public Vec3 Pos;
		public Vec3 Forward;
		public Vec3 Sacle;
		public ObjType Type;

		Transform() = default;
		const char *getName() const { return "Transform"; } 
		template <typename T>
		void visit(T &t, bool )
		{
			t.visit(1, false, "Pos", Pos);
			t.visit(2, false, "Forward", Forward);
			t.visit(3, false, "Sacle", Sacle);
			t.visit(4, false, "Type", (int&)Type);
		}
		template <typename T>
		void visit(T &t, bool ) const
		{
			t.visit(1, false, "Pos", Pos);
			t.visit(2, false, "Forward", Forward);
			t.visit(3, false, "Sacle", Sacle);
			t.visit(4, false, "Type", (int&)Type);
		}
		void swap(Transform &b)
		{
			std::swap(Pos, b.Pos);
			std::swap(Forward, b.Forward);
			std::swap(Sacle, b.Sacle);
			std::swap(Type, b.Type);
		bool operator== (const Transform &rhs) const
		{
			return Pos == rhs.Pos
				&& Forward == rhs.Forward
				&& Sacle == rhs.Sacle
				&& Type == rhs.Type;
		}
		bool operator!= (const Transform &rhs) const
		{
			return !((*this) == rhs);
		}
	}
}
namespace std
{
	inline void swap(Config::Transform &a, Config::Transform &b) { a.swap(b); }
}
