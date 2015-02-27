﻿using FineUI;
using System;

namespace CAF.Web.WebForm.Common
{
    using System.Reflection;
    using System.Web.UI;

    using CAF.Data;

    public class PageTools
    {


        /// <summary>
        /// 绑定数据到控件
        /// </summary>
        /// <param name="item"></param>
        /// <param name="model"></param>
        public static void BindControls(Control item, object model)
        {
            if (!item.HasControls())
            {
                PropertyInfo Info;
                if (item is RealTextField)
                {
                    var ctrl = (RealTextField)item;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("txt", ""));
                    if (Info != null)
                    {
                        ctrl.Text = DataMap.GetStr(Info.GetValue(model, null));
                    }
                }
                if (item is Label)
                {
                    var ctrl = (Label)item;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("lbl", ""));
                    if (Info != null)
                    {
                        ctrl.Text = DataMap.GetStr(Info.GetValue(model, null));
                    }
                }
                if (item is DropDownList)
                {
                    DropDownList ctrl = item as DropDownList;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("drop", ""));
                    if (Info != null)
                    {
                        ctrl.SelectedValue = DataMap.GetStr(Info.GetValue(model, null));
                    }
                }
                if (item is CheckBox)
                {
                    CheckBox ctrl = item as CheckBox;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("chk", ""));
                    if (Info != null)
                    {
                        ctrl.Checked = DataMap.GetStr(Info.GetValue(model, null)) == "False" ||
                        DataMap.GetStr(Info.GetValue(model, null)) == "0" ? false : true;
                    }
                }
                if (item is CheckBoxList)
                {
                    CheckBoxList ctrl = item as CheckBoxList;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("chk", ""));
                    if (Info != null)
                    {
                        PageTools.CheckBoxList(ctrl, DataMap.GetStr(Info.GetValue(model, null)));
                    }
                }
                if (item is RadioButton)
                {
                    RadioButton ctrl = item as RadioButton;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("radio", ""));
                    if (Info != null)
                    {
                        ctrl.Checked = DataMap.GetStr(Info.GetValue(model, null)) == "-1" ? false : true;
                    }
                }
                if (item is RadioButtonList)
                {
                    RadioButtonList ctrl = item as RadioButtonList;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("radio", ""));
                    if (Info != null)
                    {
                        ctrl.SelectedValue = DataMap.GetStr(Info.GetValue(model, null));
                    }
                }
                if (item is DatePicker)
                {
                    DatePicker ctrl = item as DatePicker;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("date", ""));
                    if (Info != null)
                    {
                        ctrl.Text = (DateTime.Parse(DataMap.GetStr(Info.GetValue(model, null)))).ToString("yyyy-MM-dd");
                    }
                }
            }
            else
            {
                foreach (Control c in item.Controls)
                {
                    BindControls(c, model);
                }
            }
        }

        /// <summary>
        /// 绑定控件数据到实例
        /// </summary>
        /// <param name="item"></param>
        /// <param name="model"></param>
        public static void BindModel(Control item, object model)
        {
            if (!item.HasControls())
            {
                PropertyInfo Info;
                if (item is RealTextField)
                {
                    RealTextField ctrl = item as RealTextField;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("txt", ""));
                    if (Info != null)
                    {
                        if (!(Info.Name == "Id" && string.IsNullOrWhiteSpace(ctrl.Text)))
                        {
                            Info.SetValue(model, DataMap.GetType(Info, ctrl.Text), null);
                        }
                    }
                }
                if (item is Label)
                {
                    Label ctrl = item as Label;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("lbl", ""));
                    if (Info != null && ctrl.Text != "")
                    {
                        if (!(Info.Name == "Id" && string.IsNullOrWhiteSpace(ctrl.Text)))
                        {
                            Info.SetValue(model, DataMap.GetType(Info, ctrl.Text), null);
                        }
                    }
                }
                if (item is HiddenField)
                {
                    HiddenField ctrl = item as HiddenField;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("hid", ""));
                    if (Info != null && ctrl.Text != "")
                    {
                        if (!(Info.Name == "Id" && string.IsNullOrWhiteSpace(ctrl.Text)))
                        {
                            Info.SetValue(model, DataMap.GetType(Info, ctrl.Text), null);
                        }
                    }
                }
                if (item is DatePicker)
                {
                    DatePicker ctrl = item as DatePicker;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("date", ""));
                    if (Info != null && ctrl.Text != "")
                    {
                        Info.SetValue(model, DataMap.GetType(Info, ctrl.SelectedDate.Value.ToShortDateString()), null);
                    }
                }
                if (item is DropDownList)
                {
                    DropDownList ctrl = item as DropDownList;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("drop", ""));
                    if (Info != null && !string.IsNullOrWhiteSpace(ctrl.SelectedValue))
                    {
                        Info.SetValue(model, DataMap.GetType(Info, ctrl.SelectedValue), null);
                    }
                }
                if (item is CheckBox)
                {
                    CheckBox ctrl = item as CheckBox;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("chk", ""));
                    if (Info != null)
                    {
                        if (ctrl.Checked)
                        {
                            if (Info.PropertyType == typeof(Boolean))
                            {
                                Info.SetValue(model, DataMap.GetType(Info, "True"), null);
                            }
                            else
                            {
                                Info.SetValue(model, DataMap.GetType(Info, "1"), null);
                            }
                        }
                        else
                        {
                            if (Info.PropertyType == typeof(Boolean))
                            {
                                Info.SetValue(model, DataMap.GetType(Info, "False"), null);
                            }
                            else
                            {
                                Info.SetValue(model, DataMap.GetType(Info, "0"), null);
                            }
                        }
                    }
                }
                if (item is CheckBoxList)
                {
                    CheckBoxList ctrl = item as CheckBoxList;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("chk", ""));
                    if (Info != null)
                    {
                        Info.SetValue(model, DataMap.GetType(Info, PageTools.CheckBoxList(ctrl, "")), null);
                    }
                }
                if (item is RadioButton)
                {
                    RadioButton ctrl = item as RadioButton;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("radio", ""));
                    if (Info != null)
                    {
                        if (ctrl.Checked)
                        {
                            Info.SetValue(model, DataMap.GetType(Info, "1"), null);
                        }
                        else
                        {
                            Info.SetValue(model, DataMap.GetType(Info, "0"), null);
                        }
                    }
                }
                if (item is RadioButtonList)
                {
                    RadioButtonList ctrl = item as RadioButtonList;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("radio", ""));
                    if (Info != null)
                    {
                        Info.SetValue(model, DataMap.GetType(Info, ctrl.SelectedValue), null);
                    }
                }
            }
            else
            {
                foreach (Control c in item.Controls)
                {
                    BindModel(c, model);
                }
            }
        }

        /// <summary>
        /// 复选框列表操作
        /// </summary>
        /// <param name="chkl"></param>
        /// <param name="chklCheck">不为空赋值，为空取值</param>
        /// <returns></returns>
        public static string CheckBoxList(CheckBoxList chkl, string chklCheck)
        {
            string temp = "";
            if (chklCheck == "")
            {
                for (int i = 0; i < chkl.Items.Count; i++)
                {
                    if (chkl.Items[i].Selected)
                    {
                        temp += chkl.Items[i].Value + ",";
                    }
                }
                if (temp != "")
                {
                    temp = temp.Substring(0, temp.Length - 1);
                }
            }
            else
            {
                for (int j = 0; j < chkl.Items.Count; j++)
                {
                    if (chklCheck.IsInArryString(chkl.Items[j].Value, ','))
                    {
                        chkl.Items[j].Selected = true;
                    }
                }
            }
            return temp;
        }

        /// <summary>
        /// 禁用控件
        /// </summary>
        public static void DisableControl(Control item)
        {
            if (!item.HasControls())
            {
                if (item is ControlBase)
                {
                    ControlBase ctrl = item as ControlBase;
                    ctrl.Enabled = false;
                }
            }
            else
            {
                foreach (Control i in item.Controls)
                {
                    DisableControl(i);
                }
            }
        }

        /// <summary>
        /// 清除控件绑定
        /// </summary>
        public static void ClearControls(Control item)
        {
            if (!item.HasControls())
            {
                if (item is RealTextField)
                {
                    RealTextField ctrl = item as RealTextField;
                    ctrl.Text = "";
                }
                if (item is Label)
                {
                    Label ctrl = item as Label;
                    ctrl.Text = "";
                }
                if (item is DropDownList)
                {
                    DropDownList ctrl = item as DropDownList;
                    ctrl.SelectedIndex = 0;
                }
                if (item is Tree)
                {
                    Tree ctrl = item as Tree;
                    ctrl.Nodes.Clear();
                }
                if (item is CheckBox)
                {
                    CheckBox ctrl = item as CheckBox;
                    ctrl.Checked = false;
                }
                if (item is CheckBoxList)
                {
                    CheckBoxList ctrl = item as CheckBoxList;

                    foreach (CheckItem i in ctrl.Items)
                    {
                        i.Selected = false;
                    }
                }
                if (item is RadioButton)
                {
                    RadioButton ctrl = item as RadioButton;
                    ctrl.Checked = false;
                }
                if (item is RadioButtonList)
                {
                    RadioButtonList ctrl = item as RadioButtonList;
                    foreach (RadioItem i in ctrl.Items)
                    {
                        i.Selected = false;
                    }
                    if (ctrl.Items.Count > 0)
                    {
                        ctrl.Items[0].Selected = true;
                    }
                }
            }
            else
            {
                if (item is Grid)
                {
                    Grid ctrl = item as Grid;
                    ctrl.DataSource = null;
                    ctrl.DataBind();
                }
                else
                {
                    foreach (Control c in item.Controls)
                    {
                        ClearControls(c);
                    }
                }
            }
        }

        /// <summary>
        /// 单选框列表操作
        /// </summary>
        /// <param name="rbl"></param>
        /// <param name="rblValue">不为空赋值，为空取值</param>
        /// <returns></returns>
        public static string RadioButtonList(RadioButtonList rbl, string rblValue)
        {
            string temp = "";
            if (rblValue == "")
            {
                temp = rbl.SelectedValue;
            }
            else
            {
                for (int i = 0; i < rbl.Items.Count; i++)
                {
                    if (rbl.Items[i].Value == rblValue)
                    {
                        rbl.Items[i].Selected = true;
                    }
                }
            }
            return temp;
        }
    }
}