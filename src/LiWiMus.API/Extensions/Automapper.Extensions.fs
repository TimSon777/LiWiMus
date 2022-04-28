module LiWiMus.API.Extensions

open System
open System.Linq.Expressions
open AutoMapper

type AutoMapper.IMappingExpression<'TSource, 'TDestination> with
    member this.ForMemberFs<'TMember>
            (destGetter:Expression<Func<'TDestination, 'TMember>>,
             sourceGetter:Action<IMemberConfigurationExpression<'TSource, 'TDestination, 'TMember>>) =
        this.ForMember(destGetter, sourceGetter)
        

type IMemberConfigurationExpression<'TSource, 'TDestination, 'TMember>  with
    member this.MapFromFs<'T1, 'T2 when 'T1 :> IMemberValueResolver<'TSource,'TDestination,'T2,'TMember>>
            (source: Expression<Func<'TSource, 'T2>>)  =
            this.MapFrom<'T1, 'T2>(source) 