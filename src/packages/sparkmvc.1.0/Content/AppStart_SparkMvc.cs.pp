using System.Web.Mvc;
using Spark;
using Spark.Web.Mvc;

[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.AppStart_SparkMvc), "Start")]

namespace $rootnamespace$ {
    public static class AppStart_SparkMvc {
        public static void Start() {
			var settings = new SparkSettings();

			settings.SetAutomaticEncoding(true); 

			// Note: you can change the list of namespace and assembly
			// references in Views\Shared\_global.spark

            ViewEngines.Engines.Add(new SparkViewFactory(settings));
        }
    }
}
