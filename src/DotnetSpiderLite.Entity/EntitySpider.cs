using DotnetSpiderLite.Entity.PageProcessor;
using DotnetSpiderLite.Entity.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Entity
{
    public class EntitySpider : Spider
    {
        private IList<IEntity> _entities = new List<IEntity>();
        private IEntityInterpreter _entityExtraction = new EntityInterpreter();
        private IPageEntityProcessor _pageEntityProcessor = new PageEntityProcessor();
        private IList<EntityDefine> _entityDefines = new List<EntityDefine>();

        public IList<IEntitylPipeline<IEntity>> EntitylPipelines { get; } = new List<IEntitylPipeline<IEntity>>();

        public IList<IEntity> Entities { get => _entities; }

        protected EntitySpider(params IEntity[] entities) : base()
        {
        }

        public static EntitySpider Create<TEntity>(string url, string referer = null, Dictionary<string, string> exts = null) where TEntity : IEntity
        {
            EntitySpider entitySpider = new EntitySpider();

            entitySpider.AddRequest(url, referer, exts);

            entitySpider.Entities.Add(Activator.CreateInstance<TEntity>());

            return entitySpider;
        }

        public EntitySpider AddEntity<TEntity>() where TEntity : IEntity
        {
            Entities.Add(Activator.CreateInstance<TEntity>());

            return this;
        }

        public EntitySpider AddEntitylPipeline(IEntitylPipeline<IEntity> pipeline)
        {
            this.EntitylPipelines.Add(pipeline);

            return this;
        }


        protected override void OnRunBefore()
        {
            PrepareEntities();

        }

        protected void PrepareEntities()
        {
            var entityDefines = new List<EntityDefine>();

            foreach (var entity in _entities)
            {
                var result = _entityExtraction.Handle(entity);
                entityDefines.Add(result);
            }

            _entityDefines = entityDefines;
        }

        protected override void OnPageProcessor(Page page)
        {
            foreach (var define in _entityDefines)
            {
                var entity = _pageEntityProcessor.Process(define.Entity, define, page);

            }
        }
    }
}
