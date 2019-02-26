//本文件由工具生成，请勿直接修改
#pragma once
#include <string>
#include <vector>
#include <map>
#include <algorithm>
#include "Vec3.h"
namespace Config
{
	struct Box
	{
		public Vec3 Size;

		Box() = default;
		const char *getName() const { return "Box"; } 
		template <typename T>
		void visit(T &t, bool )
		{
			t.visit(1, false, "Size", Size);
		}
		template <typename T>
		void visit(T &t, bool ) const
		{
			t.visit(1, false, "Size", Size);
		}
		void swap(Box &b)
		{
			std::swap(Size, b.Size);
		bool operator== (const Box &rhs) const
		{
			return Size == rhs.Size;
		}
		bool operator!= (const Box &rhs) const
		{
			return !((*this) == rhs);
		}
	}
}
namespace std
{
	inline void swap(Config::Box &a, Config::Box &b) { a.swap(b); }
}
