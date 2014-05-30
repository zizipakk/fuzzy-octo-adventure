using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
//using System.Data.Objects;
//using System.Data.Objects.DataClasses;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.ComponentModel;
//using LinqKit;
//using Tax.Data.Models;

namespace JQGrid.Helpers
{

    public static class LinqExtensions
    {
        //public static void ApplyGridChanges<T>(this System.Data.Entity.Core.Objects.ObjectSet<T> tbl, T r, string id, string oper)
        //    where T : EntityObject
        //public static void ApplyGridChanges<T>(this FunkContext tbl, T r, string id, string oper)
        //{

        //    Guid strGuid = (oper == "add" ? Guid.NewGuid() : new Guid(id));

        //    Type etype = r.GetType();
        //    Type guidType = strGuid.GetType();

        //    var propertyInfo = etype.GetProperty(tbl.EntitySet.ElementType.KeyMembers[0].ToString());
        //    var propertyType = propertyInfo.PropertyType;

        //    var instanceParameter = Expression.Parameter(etype, "instance");
        //    var valueParameter = Expression.Parameter(guidType, "value");

        //    var lambdaSet = Expression.Lambda<Action<T, Guid>>(
        //       Expression.Assign(
        //          Expression.Property(Expression.Convert(instanceParameter, etype), propertyInfo),
        //          Expression.Convert(valueParameter, propertyType)),
        //       instanceParameter,
        //       valueParameter
        //       );

        //    var setID = lambdaSet.Compile();

        //    //var equalTarget = Expression.Constant(strGuid,guidType);
        //    //var EqualsTo = Expression.Equal(Expression.Property(instanceParameter, propertyInfo), equalTarget);
        //    var EqualsTo = Expression.Equal(Expression.Property(instanceParameter, propertyInfo),
        //                                    Expression.Constant(strGuid, guidType));
        //    var lambdaGet = Expression.Lambda<Func<T, bool>>(EqualsTo, instanceParameter);

        //    switch (oper)
        //    {
        //        case "edit":
        //            //r.EntityKey = new EntityKey(string.Format("{0}.{1}",tbl.EntitySet.EntityContainer,tbl.EntitySet.Name),
        //            //                        tbl.EntitySet.ElementType.KeyMembers[0].ToString(), strGuid);
        //            //r.GetType().GetProperty(tbl.EntitySet.ElementType.KeyMembers[0].ToString()).SetValue(r, strGuid, null);
        //            //setIdDelegate(r, strGuid);

        //            setID(r, strGuid);
        //            //tbl.Attach(r);

        //            tbl.Entry<T>(r).State = System.Data.Entity.EntityState.Modified;
        //            //tbl.Context.ObjectStateManager.ChangeObjectState(r, System.Data.Entity.EntityState.Modified);
        //            //((IObjectContextAdapter)tbl).ObjectContext.ApplyCurrentValues(r);
        //            tbl.Context.ApplyCurrentValues(etype.Name, r);
        //            break;
        //        case "del":
        //            //var cond = getIdDelegate;
        //            //r = (from x in tbl.AsExpandable() where cond.Invoke(x)==strGuid select x).FirstOrDefault();

        //            r = (from x in tbl.AsExpandable() where lambdaGet.Invoke(x) select x).FirstOrDefault();

        //            if (null == r)
        //            {
        //                //todo: hibakezelés
        //                return;
        //            }

        //            tbl.DeleteObject(r);
        //            break;
        //        case "add":
        //            //r.EntityKey = new EntityKey(string.Format("{0}.{1}", tbl.EntitySet.EntityContainer, tbl.EntitySet.Name),
        //            //                        tbl.EntitySet.ElementType.KeyMembers[0].ToString(), strGuid);
        //            //r.GetType().GetProperty(tbl.EntitySet.ElementType.KeyMembers[0].ToString()).SetValue(r, strGuid, null);
        //            //setIdDelegate(r, strGuid);

        //            setID(r, strGuid);
        //            tbl.AddObject(r);
        //            break;
        //        default:
        //            //todo: hibakelezés
        //            break;
        //    }
        //    return;
        //}        
        
        public static T[] GridPage<T>(this IQueryable<T> rs, GridSettings grid, out JsonResult result)
        {

            if (grid.IsSearch)
            {
                //And
                if (grid.Where.groupOp == "AND")
                {
                    foreach (var rule in grid.Where.rules)
//TODO: checkbox = Mind értéknél bedőlt, mert nem tudja az üres value típusát megállíapítani, 
                    //rs = rs.Where(
                    //   rule.field, rule.data,
                    //   (WhereOperation)StringEnum.Parse(typeof(WhereOperation), rule.op));
                        if (rule.data.ToString() != "\"\"")
                            rs = rs.Where(
                               rule.field, rule.data,
                               (WhereOperation)StringEnum.Parse(typeof(WhereOperation), rule.op));
                }
                else
                {
                    //Or
                    var temp = (new List<T>()).AsQueryable();
                    foreach (var rule in grid.Where.rules)                    
//TODO: checkbox = Mind értéknél bedőlt
                        //var t = rs.Where(
                        //rule.field, rule.data,
                        //(WhereOperation)StringEnum.Parse(typeof(WhereOperation), rule.op));
                        //temp = temp.Concat(t);
                        if (rule.data.ToString() != "\"\"")
                        {
                            var t = rs.Where(
                            rule.field, rule.data,
                            (WhereOperation)StringEnum.Parse(typeof(WhereOperation), rule.op)).ToList();
                            temp = temp.Concat(t);
                        }
                    
                    //remove repeating records
//TODO: checkbox = Mind értéknél bedőlt
                    //rs = temp.Distinct();
                    if (temp.Count() != 0)
                        rs = temp.Distinct();
                }
            }

            //sorting, TODO: ha nincs rendezési szempont, ne dőljön el
            if (null != rs && grid.SortColumn != "")
            {
                rs = rs.OrderBy(grid.SortColumn, grid.SortOrder);
            }

            //count
            var count = rs.Count();

            //paging
            result = new JsonResult
            {
                total = (int)Math.Ceiling((double)count / grid.PageSize),
                page = grid.PageIndex,
                records = count
            };

            return rs.AsQueryable().Skip((grid.PageIndex - 1) * grid.PageSize).Take(grid.PageSize).ToArray();
        }
        
        /// <summary>Orders the sequence by specific column and direction.</summary>
        /// <param name="query">The query.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="ascending">if set to true [ascending].</param>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string sortColumn, string direction)
        {

            ParameterExpression parameter = Expression.Parameter(query.ElementType, "p");

            var asortColumn = sortColumn.Split(',');

            MethodCallExpression result = null;
            MemberExpression memberAccess = null;
            string methodName = string.Empty;

            foreach (string s in asortColumn)
            {
                methodName = string.Format((string.Empty == methodName ? "OrderBy{0}" : "ThenBy{0}"),
                    direction.ToLower() == "asc" ? "" : "descending");

                memberAccess = null;

                foreach (var property in s.Split('.'))
                    memberAccess = MemberExpression.Property
                       (memberAccess ?? (parameter as Expression), property);
                LambdaExpression orderByLambda = Expression.Lambda(memberAccess, parameter);

                result = Expression.Call(
                          typeof(Queryable),
                          methodName,
                          new[] { query.ElementType, memberAccess.Type },
                          query.Expression,
                          Expression.Quote(orderByLambda));

                query = query.Provider.CreateQuery<T>(result);
            }


            return query.Provider.CreateQuery<T>(result);
        }


        public static IQueryable<T> Where<T>(this IQueryable<T> query,
            string column, object value, WhereOperation operation)
        {
            if (string.IsNullOrEmpty(column)
                )
                return query;

            ParameterExpression parameter = Expression.Parameter(query.ElementType, "p");

            MemberExpression memberAccess = null;
            foreach (var property in column.Split('.'))
                memberAccess = MemberExpression.Property(memberAccess ?? (parameter as Expression), property);

            //change param value type
            //necessary to getting bool from string
            ConstantExpression filter = null;

            if (memberAccess.Type.IsEnum)
            { //Ha enum, akkor enumként parse-oljuk
                value = Enum.Parse(memberAccess.Type, value.ToString());
            }

            //Guid
            if (memberAccess.Type.FullName.Equals("System.Guid",StringComparison.OrdinalIgnoreCase))
            { // ha guid akkor guid-ként parse-oljuk
                value = Guid.Parse(value.ToString());
            }

            //Datetime
            if (memberAccess.Type.FullName.Equals("System.DateTime", StringComparison.OrdinalIgnoreCase))
            {
                value = DateTime.Parse(value.ToString());    
            }

            //Datetime?
            //if (memberAccess.Type.GenericTypeArguments.FirstOrDefault().FullName.Equals("System.DateTime", StringComparison.OrdinalIgnoreCase))
            //{
            //    value = string.IsNullOrWhiteSpace(value.ToString()) ? (DateTime?)null : (DateTime?)DateTime.Parse(value.ToString());
            //    filter = Expression.Constant(value);
            //}

            //Timespan
            if (memberAccess.Type.FullName.Equals("System.TimeSpan", StringComparison.OrdinalIgnoreCase))
            {
                value = TimeSpan.Parse(value.ToString());
            }

            //float
            if (memberAccess.Type.FullName.Equals("System.Single", StringComparison.OrdinalIgnoreCase))
            {
                value = float.Parse(value.ToString(), CultureInfo.InvariantCulture); 
            }

            if (IsNullableType(memberAccess.Type))//nullable
            {
                TypeConverter conv = TypeDescriptor.GetConverter(memberAccess.Type);
                var conValue = (DateTime?)conv.ConvertFrom(value.ToString());
                filter = Expression.Constant(conValue);
            }
            else
            {
                filter = Expression.Constant(Convert.ChangeType(value, memberAccess.Type));                 
            }

            //switch operation
            Expression condition = null;
            LambdaExpression lambda = null;
            ConstantExpression zero = Expression.Constant(0, typeof(int));
            switch (operation)
            {
                //equal ==
                case WhereOperation.Equal:
                    //condition = Expression.Equal(memberAccess, filter);
                    condition = LinqEqual(memberAccess, filter);
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                //not equal !=
                case WhereOperation.NotEqual:
                    //condition = Expression.NotEqual(memberAccess, filter);
                    condition = LinqNotEqual(memberAccess, filter);
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                //string.Contains()
                case WhereOperation.Contains:
                    condition = Expression.Call(memberAccess,
                        typeof(string).GetMethod("Contains"),
                        Expression.Constant(value));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case WhereOperation.LessThan:
                    if (memberAccess.Type != typeof(string))
                    {
                        //condition = Expression.LessThan(memberAccess, filter);
                        condition = LinqLessThan(memberAccess, filter);
                    }
                    else
                    {
                        condition = Expression.LessThan(
                            Expression.Call(memberAccess,
                                typeof(string).GetMethod("Compare"),
                                Expression.Constant(value)),
                                zero
                            );
                    }
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case WhereOperation.LessThanOrEqual:
                    if (memberAccess.Type != typeof(string))
                    {
                        //condition = Expression.LessThanOrEqual(memberAccess, filter);
                        condition = LinqLessThanOrEqual(memberAccess, filter);
                    }
                    else
                    {
                        condition = Expression.LessThanOrEqual(
                            Expression.Call(memberAccess,
                                typeof(string).GetMethod("Compare"),
                                Expression.Constant(value)),
                                zero
                            );
                    }
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case WhereOperation.GreaterThan:
                    if (memberAccess.Type != typeof(string))
                    {
                        //condition = Expression.GreaterThan(memberAccess, filter);
                        condition = LinqGreaterThan(memberAccess, filter);
                    }
                    else
                    {
                        condition = Expression.GreaterThan(
                            Expression.Call(memberAccess,
                                typeof(string).GetMethod("Compare"),
                                Expression.Constant(value)),
                                zero
                            );
                    }
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case WhereOperation.GreaterThanOrEqual:
                    if (memberAccess.Type != typeof(string))
                    {
                        //condition = Expression.GreaterThanOrEqual(memberAccess, filter);
                        condition = LinqGreaterThanOrEqual(memberAccess, filter);
                    }
                    else
                    {
                        condition = Expression.GreaterThanOrEqual(
                            Expression.Call(memberAccess,
                                typeof(string).GetMethod("Compare"),
                                Expression.Constant(value)),
                                zero
                            );
                    }
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case WhereOperation.BeginsWith:
                    condition = Expression.Call(memberAccess,
                        typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }),
                        Expression.Constant(value));
                        lambda = Expression.Lambda(condition, parameter);
                        break;
                case WhereOperation.DoesNotBeginsWith:
                    condition = Expression.Not(
                        Expression.Call(memberAccess,
                        typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }),
                        Expression.Constant(value)));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case WhereOperation.In:
                    //ConstantExpression array = Expression.Constant(value.ToString().Split(',').ToList() , typeof(List<string>));
                    //MethodInfo any = typeof(List<string>).GetMethod("Contains", new Type[] { typeof(string) });
                    //condition = Expression.Call(array,
                    //    any,
                    //    memberAccess);
                    //lambda = Expression.Lambda(condition, parameter);
                    condition = Expression.Call(Expression.Constant(value),
                        typeof(string).GetMethod("Contains"),
                        memberAccess);
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case WhereOperation.NotIn:
                    //ConstantExpression arrayNotIn = Expression.Constant(value.ToString().Split(',').ToList() , typeof(List<string>));
                    //MethodInfo anyNotIn = typeof(List<string>).GetMethod("Contains", new Type[] { typeof(string) });
                    //condition = Expression.Not(
                    //    Expression.Call(arrayNotIn,
                    //    anyNotIn,
                    //    memberAccess));
                    condition = Expression.Not(
                        Expression.Call(Expression.Constant(value),
                        typeof(string).GetMethod("Contains"),
                        memberAccess));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case WhereOperation.EndsWith:
                    condition = Expression.Call(memberAccess,
                        typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }),
                        Expression.Constant(value));
                        lambda = Expression.Lambda(condition, parameter);
                        break;
                case WhereOperation.DoesNotEndsWith:
                    condition = Expression.Not(
                        Expression.Call(memberAccess,
                        typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }),
                        Expression.Constant(value)));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case WhereOperation.DoesNotContain:
                    condition = Expression.Not(
                        Expression.Call(memberAccess,
                        typeof(string).GetMethod("Contains"),
                        Expression.Constant(value)));
                    lambda = Expression.Lambda(condition, parameter);
                    break;            
            }
            
            MethodCallExpression result = Expression.Call(
                   typeof(Queryable), "Where",
                   new[] { query.ElementType },
                   query.Expression,
                   lambda);

            return query.Provider.CreateQuery<T>(result);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        //public static Nullable<T> ToNullable<T>(this string s) where T : struct
        //{
        //    Nullable<T> result = new Nullable<T>();
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(s))
        //        {
        //            TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
        //            result = (T)conv.ConvertFrom(s);
        //        }
        //        else
        //        {
        //            result = null;
        //        }
        //    }
        //    catch { }
        //    return result;
        //}

        private static Expression LinqEqual(Expression e1, Expression e2)
        {
            if (IsNullableType(e1.Type) && !IsNullableType(e2.Type))
                e2 = Expression.Convert(e2, e1.Type);
            else if (!IsNullableType(e1.Type) && IsNullableType(e2.Type))
                e1 = Expression.Convert(e1, e2.Type);
            return Expression.Equal(e1, e2);
        }
        private static Expression LinqNotEqual(Expression e1, Expression e2)
        {
            if (IsNullableType(e1.Type) && !IsNullableType(e2.Type))
                e2 = Expression.Convert(e2, e1.Type);
            else if (!IsNullableType(e1.Type) && IsNullableType(e2.Type))
                e1 = Expression.Convert(e1, e2.Type);
            return Expression.NotEqual(e1, e2);
        }
        private static Expression LinqLessThan(Expression e1, Expression e2)
        {
            if (IsNullableType(e1.Type) && !IsNullableType(e2.Type))
                e2 = Expression.Convert(e2, e1.Type);
            else if (!IsNullableType(e1.Type) && IsNullableType(e2.Type))
                e1 = Expression.Convert(e1, e2.Type);
            return Expression.LessThan(e1, e2);
        }
        private static Expression LinqLessThanOrEqual(Expression e1, Expression e2)
        {
            if (IsNullableType(e1.Type) && !IsNullableType(e2.Type))
                e2 = Expression.Convert(e2, e1.Type);
            else if (!IsNullableType(e1.Type) && IsNullableType(e2.Type))
                e1 = Expression.Convert(e1, e2.Type);
            return Expression.LessThanOrEqual(e1, e2);
        }
        private static Expression LinqGreaterThan(Expression e1, Expression e2)
        {
            if (IsNullableType(e1.Type) && !IsNullableType(e2.Type))
                e2 = Expression.Convert(e2, e1.Type);
            else if (!IsNullableType(e1.Type) && IsNullableType(e2.Type))
                e1 = Expression.Convert(e1, e2.Type);
            return Expression.GreaterThan(e1, e2);
        }
        private static Expression LinqGreaterThanOrEqual(Expression e1, Expression e2)
        {
            if (IsNullableType(e1.Type) && !IsNullableType(e2.Type))
                e2 = Expression.Convert(e2, e1.Type);
            else if (!IsNullableType(e1.Type) && IsNullableType(e2.Type))
                e1 = Expression.Convert(e1, e2.Type);
            return Expression.GreaterThanOrEqual(e1, e2);
        }

        private static bool IsNullableType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }

    //public static class QueryHelper
    //{
    //    private static string GetPath<T>(Expression<Func<T, object>> memberExpression)
    //    {
    //        var bodyString = memberExpression.Body.ToString();
    //        string path = bodyString.Remove(0, memberExpression.Parameters[0].Name.Length + 1);
    //        return path;
    //    }

    //    public static DbQuery<T> Include<T>(this DbQuery<T> query, Expression<Func<T, object>> memberExpression)
    //    {
    //        string includePath = GetPath<T>(memberExpression);
    //        return query.Include(includePath);
    //    }
    //}


}
