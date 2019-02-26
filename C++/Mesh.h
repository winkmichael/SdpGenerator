//本文件由工具生成，请勿直接修改
#pragma once
#include <string>
#include <vector>
#include <map>
#include <algorithm>
#include "Vec3.h"
#include "Transform.h"
namespace Config
{
	struct Mesh
	{
		public std::vector<Vec3> Vesters;
		public std::vector<int32_t> Triangle;
		public Transform Trans;

		Mesh() = default;
		const char *getName() const { return "Mesh"; } 
		template <typename T>
		void visit(T &t, bool )
		{
			t.visit(1, false, "Vesters", Vesters);
			t.visit(2, false, "Triangle", Triangle);
			t.visit(3, false, "Trans", Trans);
		}
		template <typename T>
		void visit(T &t, bool ) const
		{
			t.visit(1, false, "Vesters", Vesters);
			t.visit(2, false, "Triangle", Triangle);
			t.visit(3, false, "Trans", Trans);
		}
		void swap(Mesh &b)
		{
			std::swap(Vesters, b.Vesters);
			std::swap(Triangle, b.Triangle);
			std::swap(Trans, b.Trans);
		bool operator== (const Mesh &rhs) const
		{
			return Vesters == rhs.Vesters
				&& Triangle == rhs.Triangle
				&& Trans == rhs.Trans;
		}
		bool operator!= (const Mesh &rhs) const
		{
			return !((*this) == rhs);
		}
	}
}
namespace std
{
	inline void swap(Config::Mesh &a, Config::Mesh &b) { a.swap(b); }
}
