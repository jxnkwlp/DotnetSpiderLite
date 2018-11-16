using DotnetSpiderLite.Entity.PageProcessor;
using DotnetSpiderLite.Entity.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Entity
{
    public class EntitySpider
    {



        public EntitySpider AddPageModel<T>()
        {

            return this;
        }

        public EntitySpider AddModelPipeline<T>(IModelPipeline<T> pipeline)
        {


            return this;
        }

    }
}
