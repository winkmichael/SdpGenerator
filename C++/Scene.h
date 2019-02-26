//本文件由工具生成，请勿直接修改
#pragma once
#include <string>
#include <vector>
#include <map>
#include <algorithm>
#include "Mesh.h"
#include "Box.h"
#include "ObjType.h"
namespace Config
{
	struct Scene
	{
		public std::vector<Mesh> Meshs;
		public std::vector<Box> Boxs;
		public std::map<int32_t,int32_t> MapTest;
		public std::map<ObjType,int32_t> MapEnum;

		Scene() = default;
		const char *getName() const { return "Scene"; } 
		template <typename T>
		void visit(T &t, bool )
		{
			t.visit(1, false, "Meshs", Meshs);
			t.visit(2, false, "Boxs", Boxs);
			t.visit(3, false, "MapTest", MapTest);
			t.visit(4, false, "MapEnum", MapEnum);
		}
		template <typename T>
		void visit(T &t, bool ) const
		{
			t.visit(1, false, "Meshs", Meshs);
			t.visit(2, false, "Boxs", Boxs);
			t.visit(3, false, "MapTest", MapTest);
			t.visit(4, false, "MapEnum", MapEnum);
		}
		void swap(Scene &b)
		{
			std::swap(Meshs, b.Meshs);
			std::swap(Boxs, b.Boxs);
			std::swap(MapTest, b.MapTest);
			std::swap(MapEnum, b.MapEnum);
		bool operator== (const Scene &rhs) const
		{
			return Meshs == rhs.Meshs
				&& Boxs == rhs.Boxs
				&& MapTest == rhs.MapTest
				&& MapEnum == rhs.MapEnum;
		}
		bool operator!= (const Scene &rhs) const
		{
			return !((*this) == rhs);
		}
	}
}
namespace std
{
	inline void swap(Config::Scene &a, Config::Scene &b) { a.swap(b); }
}
