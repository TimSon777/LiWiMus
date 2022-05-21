﻿namespace LiWiMus.Web.MVC.Areas.Search.ViewModels;

public class SearchViewModel
{
    private string _title;
    public string Title
    {
        get => _title;
        // ReSharper disable once ConstantNullCoalescingCondition
        set => _title = value ?? "";
    }

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public int Page { get; set; }
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public int Take { get; set; }

    // ReSharper disable once MemberCanBePrivate.Global
    public SearchViewModel()
    {
        _title = "";
        Page = 1;
        Take = 2;
    }

    public static readonly SearchViewModel Default = new();
}