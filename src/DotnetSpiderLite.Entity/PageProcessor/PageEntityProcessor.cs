using DotnetSpiderLite.Entity.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace DotnetSpiderLite.Entity.PageProcessor
{
    public class PageEntityProcessor : IPageEntityProcessor
    {
        /// <summary>
        ///  1, 匹配 TargetUrl，判断是否需要解析HTML
        ///  2，匹配 HelperUrl，判断是否需要加入到请求列表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="entityDefine"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public TEntity Process<TEntity>(TEntity entity, EntityDefine entityDefine, Page page) where TEntity : IEntity
        {
            if (string.IsNullOrEmpty(page.Content))
                return entity;

            // 查找所有链接 
            var allLinkEle = page.Selector?.SelectorAll("a", Html.HtmlSelectorPathType.Css);

            List<string> links = new List<string>();

            if (allLinkEle != null)
            {
                var r = allLinkEle.Select(t => t.GetAttribute("href")).Where(t => !string.IsNullOrEmpty(t)).Distinct().ToList();

                if (r.Count > 0)
                    links.AddRange(r);
            }

            // 匹配新请求
            if (entityDefine.HelperUrls?.Length > 0)
            {
                foreach (var url in entityDefine.HelperUrls)
                {
                    foreach (var link in links)
                    {
                        if (url == link)
                        {
                            continue;
                        }

                        Regex regex = new Regex(url);

                        if (regex.IsMatch(link))
                        {
                            page.AddTargetRequest(link, page.Response.Request.Uri.ToString());
                        }
                    }
                }
            }

            if (entityDefine.TargetUrls == null || entityDefine.TargetUrls.Length == 0)
                return entity;


            foreach (var url in entityDefine.TargetUrls)
            {
                foreach (var link in links)
                {
                    if (url == link)
                    {
                        continue;
                    }

                    Regex regex = new Regex(url);

                    if (regex.IsMatch(link))
                    {
                        page.AddTargetRequest(link, page.Response.Request.Uri.ToString());
                    }
                }
            }

            // 匹配内容

            var requestUrl = page.Response.Request.Uri.ToString();

            foreach (var url in entityDefine.TargetUrls)
            {
                Regex regex = new Regex(url);

                if (regex.IsMatch(requestUrl))
                {
                    MatchSelector(entityDefine, entity, page);
                }
            }

            return entity;
        }

        private void MatchSelector<TEntity>(EntityDefine entityDefine, TEntity entity, Page page)
        {
            if (entityDefine.Properties != null)
            {
                foreach (var property in entityDefine.Properties)
                {
                    var propertyName = property.Name;

                    foreach (var attribute in property.Selectors)
                    {
                        var selectValue = GetSelector(attribute, page);

                        Helper.Helper.SetProperty(entity, propertyName, selectValue);

                    }

                }
            }
        }

        private string GetSelector(SelectorAttribute attribute, Page page)
        {
            if (!string.IsNullOrEmpty(attribute.Path))
            {
                var element = page.Selector.Selector(attribute.Path, attribute.PathType);

                if (element == null)
                    return null;

                if (attribute.SelectorMatch == SelectorMatch.InnerHtml)
                {
                    return element.InnerHtml;
                }
                else if (attribute.SelectorMatch == SelectorMatch.InnerText)
                {
                    return element.InnerText;
                }
                else if (attribute.SelectorMatch == SelectorMatch.OuterHtml)
                {
                    return element.OuterHtml;
                }

                if (!string.IsNullOrEmpty(attribute.FromAttribute))
                {
                    return element.GetAttribute(attribute.FromAttribute);
                }
            }

            return null;
        }

        //private IList<EntityDefine> MatchEntities(string url)
        //{
        //    var result = new List<EntityDefine>();

        //    foreach (var entity in _entityResult)
        //    {
        //        if (entity.TargetUrls != null)
        //        {
        //            foreach (var targetUrl in entity.TargetUrls)
        //            {
        //                if (Regex.IsMatch(url, targetUrl))
        //                {
        //                    if (!result.Contains(entity))
        //                        result.Add(entity);
        //                }
        //            }
        //        }
        //    }

        //    return result;
        //}

    }
}
