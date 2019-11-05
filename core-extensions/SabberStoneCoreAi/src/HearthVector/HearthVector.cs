using SabberStoneCore.Model.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SabberStoneCoreAi.HearthVector
{
	public interface IHearthVector
	{
		bool IsNull { get; set; }
		List<int> Vector { get; set; }
		List<string> GetPropertyNames();
		//bool SetSpecialProperty(Entity e, string propName);
	}

	public abstract class HearthVector : IHearthVector
	{
		public bool IsNull { get; set; } = false;
		public List<int> Vector { get; set; } = new List<int>();

		public HearthVector()
		{
			IsNull = true;
		}

		public PropertyInfo[] GetProperties()
		{
			return GetType().GetProperties();
		}

		public List<string> GetPropertyNames()
		{
			var propNames = new List<string>();
			PropertyInfo[] props = GetType().GetProperties();

			foreach (PropertyInfo p in props)
				propNames.Add(p.Name);

			return propNames;
		}

		//public virtual bool SetSpecialProperty(Entity e, string propName)
		//{
		//	throw new NotImplementedException();
		//}
	}
}
