using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;

namespace CAF.Web.WebForm.Common
{

    using CAF.Data;
    using CAF.Utility;

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
        public static void BindModel(Control item, IBusinessBase model)
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
                        if (!(item is HiddenField) && skipProperties(model, Info)
                            && ((Info.Name == "Id" && ctrl.Text.ToGuid() != Guid.Empty)
                            || Info.Name != "Id"))
                        {
                            Info.SetValue(model, DataMap.GetType(Info, ctrl.Text), null);
                        }
                    }
                }
                if (item is Label)
                {
                    var ctrl = (Label)item;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("lbl", ""));
                    if (Info != null && ctrl.Text != "" && skipProperties(model, Info) &&
                        !(Info.Name == "Id" && String.IsNullOrWhiteSpace(ctrl.Text)))
                    {
                        Info.SetValue(model, DataMap.GetType(Info, ctrl.Text), null);
                    }
                }
                if (item is DatePicker)
                {
                    var ctrl = (DatePicker)item;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("date", ""));
                    if (Info != null && ctrl.Text != "" && skipProperties(model, Info))
                    {
                        Info.SetValue(model, DataMap.GetType(Info, ctrl.SelectedDate.Value.ToShortDateString()), null);
                    }
                }
                if (item is DropDownList)
                {
                    var ctrl = (DropDownList)item;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("drop", ""));
                    if (Info != null && !String.IsNullOrWhiteSpace(ctrl.SelectedValue)
                        && skipProperties(model, Info))
                    {
                        Info.SetValue(model, DataMap.GetType(Info, ctrl.SelectedValue), null);
                    }
                }
                if (item is CheckBox)
                {
                    var ctrl = (CheckBox)item;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("chk", ""));
                    if (Info != null && skipProperties(model, Info))
                    {
                        if (ctrl.Checked)
                        {
                            Info.SetValue(model,
                                Info.PropertyType == typeof(Boolean)
                                    ? DataMap.GetType(Info, "True")
                                    : DataMap.GetType(Info, "1"), null);
                        }
                        else
                        {
                            Info.SetValue(model,
                                Info.PropertyType == typeof(Boolean)
                                    ? DataMap.GetType(Info, "False")
                                    : DataMap.GetType(Info, "0"), null);
                        }
                    }
                }
                if (item is CheckBoxList)
                {
                    var ctrl = (CheckBoxList)item;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("chk", ""));
                    if (Info != null && skipProperties(model, Info))
                    {
                        Info.SetValue(model, DataMap.GetType(Info, PageTools.CheckBoxList(ctrl, "")), null);
                    }
                }
                if (item is RadioButton)
                {
                    var ctrl = (RadioButton)item;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("radio", ""));
                    if (Info != null && skipProperties(model, Info))
                    {
                        Info.SetValue(model, ctrl.Checked ? DataMap.GetType(Info, "1") : DataMap.GetType(Info, "0"),
                            null);
                    }
                }
                if (item is RadioButtonList)
                {
                    var ctrl = (RadioButtonList)item;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("radio", ""));
                    if (Info != null && skipProperties(model, Info))
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

        private static bool skipProperties(IBusinessBase model, PropertyInfo Info)
        {
            return model.SkipedProperties == null ||
                   (model.SkipedProperties != null && !model.SkipedProperties.Contains(Info.Name))
            ;
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
                    ctrl.ClearInvalid();
                }
                if (item is Label)
                {
                    Label ctrl = item as Label;
                    ctrl.Text = "";
                    ctrl.ClearInvalid();
                }
                if (item is DropDownList)
                {
                    DropDownList ctrl = item as DropDownList;
                    ctrl.SelectedIndex = 0;
                    ctrl.ClearInvalid();
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
                    ctrl.ClearInvalid();
                }
                if (item is CheckBoxList)
                {
                    CheckBoxList ctrl = item as CheckBoxList;
                    foreach (CheckItem i in ctrl.Items)
                    {
                        i.Selected = false;
                    }
                    ctrl.ClearInvalid();
                }
                if (item is RadioButton)
                {
                    RadioButton ctrl = item as RadioButton;
                    ctrl.Checked = false;
                    ctrl.ClearInvalid();
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
                    ctrl.ClearInvalid();
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
            var temp = "";
            if (rblValue == "")
            {
                temp = rbl.SelectedValue;
            }
            else
            {
                foreach (var t in rbl.Items.Where(t => t.Value == rblValue))
                {
                    t.Selected = true;
                }
            }
            return temp;
        }

        /// <summary>
        /// 绑定单选列表
        /// </summary>
        /// <param name="radio"></param>
        public static void BindRadioButton<T>(RadioButtonList radio)
        {
            if (radio.Items.Count == 0)
            {
                foreach (var item in CAF.Utility.Enum.GetItems<T>())
                {
                    radio.Items.Add(new RadioItem() { Text = item.Text, Value = item.Value.ToString() });
                }
            }
            radio.SelectedIndex = 0;
        }

        /// <summary>
        /// 绑定下拉列表
        /// </summary>
        /// <param name="items"></param>
        /// <param name="drop"></param>
        /// <param name="selectItem">选中项</param>
        /// <param name="defaultitemValue"></param>
        public static void BindDropdownList(List<ListItem> items, DropDownList drop, string selectItem,
            string defaultitemValue = "00000000-0000-0000-0000-000000000000")
        {

            drop.Items.Clear();
            var item = new ListItem { Text = "请选择", Value = defaultitemValue };
            drop.Items.Add(item);
            items.ForEach(i => drop.Items.Add(i));
            selectItem.IfIsNotNullOrEmpty(r => drop.SelectedValue = r);
            drop.EnableSimulateTree = true;
        }

        /// <summary>
        /// 绑定下拉列表
        /// </summary>
        /// <param name="items"></param>
        /// <param name="drop"></param>
        /// <param name="selectItem"></param>
        /// <param name="defaultitemValue"></param>
        public static void BindDropdownList(List<KeyValueItem<Guid, string>> items, DropDownList drop, string selectItem,
            string defaultitemValue = "00000000-0000-0000-0000-000000000000")
        {

            drop.Items.Clear();
            var item = new ListItem { Text = "请选择", Value = defaultitemValue };
            drop.Items.Add(item);
            items.ForEach(i => drop.Items.Add(new ListItem { Text = i.Value, Value = i.Key.ToString() }));
            drop.SelectedValue = selectItem;
        }

        /// <summary>
        /// 绑定下拉列表
        /// </summary>
        /// <param name="enums"></param>
        /// <param name="drop"></param>
        public static void BindDropdownList<T>(DropDownList drop)
        {
            drop.Items.Clear();
            drop.Items.Add("请选择", "");
            CAF.Utility.Enum.GetItems<T>().ForEach(i => drop.Items.Add(new ListItem() { Text = i.Text, Value = i.Value.ToString() }));
        }



        /// <summary>
        /// 创建一个 6 位的随机数
        /// </summary>
        /// <returns></returns>
        public static string GenerateRandomCode()
        {
            string s = String.Empty;
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                s += random.Next(10).ToString();
            }
            return s;
        }
    }
}