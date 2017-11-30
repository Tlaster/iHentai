#pragma once

namespace WaterFallView
{
	[Windows::Foundation::Metadata::WebHostHidden()]
	public value struct VisualWindow sealed
	{
	public:
		double Offset;
		double Length;
	};

	[Windows::Foundation::Metadata::WebHostHidden()]
	public ref class VisualWindowExtension sealed
	{
	public:
		static double GetEndOffset(VisualWindow window)
		{
			return window.Offset + window.Length;
		}
		static bool Contain(VisualWindow window, VisualWindow otherWindow);
		static bool GetIntersection(VisualWindow window, VisualWindow otherWindow, VisualWindow* intersectionWindow);
		static bool GetUnion(VisualWindow window, VisualWindow otherWindow, VisualWindow* unionWindow);
		static bool IsEmpty(VisualWindow window);
	private:
		VisualWindowExtension();
	};
}
