using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;

namespace CAF.Web.WebForm.Common
{
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
                        ctrl.Text = Mapper2.GetStr(Info.GetValue(model, null));
                    }
                }
                if (item is Label)
                {
                    var ctrl = (Label)item;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("lbl", ""));
                    if (Info != null)
                    {
                        ctrl.Text = Mapper2.GetStr(Info.GetValue(model, null));
                    }
                }
                if (item is DropDownList)
                {
                    var ctrl = item as DropDownList;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("drop", ""));
                    if (Info != null)
                    {
                        ctrl.SelectedValue = Mapper2.GetStr(Info.GetValue(model, null));
                    }
                }
                if (item is CheckBox)
                {
                    var ctrl = item as CheckBox;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("chk", ""));
                    if (Info != null)
                    {
                        ctrl.Checked = Mapper2.GetStr(Info.GetValue(model, null)) != "False"
                            && Mapper2.GetStr(Info.GetValue(model, null)) != "0";
                    }
                }
                if (item is CheckBoxList)
                {
                    var ctrl = item as CheckBoxList;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("chk", ""));
                    if (Info != null)
                    {
                        PageTools.CheckBoxList(ctrl, Mapper2.GetStr(Info.GetValue(model, null)));
                    }
                }
                if (item is RadioButton)
                {
                    var ctrl = item as RadioButton;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("radio", ""));
                    if (Info != null)
                    {
                        ctrl.Checked = Mapper2.GetStr(Info.GetValue(model, null)) != "-1";
                    }
                }
                if (item is RadioButtonList)
                {
                    var ctrl = item as RadioButtonList;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("radio", ""));
                    if (Info != null)
                    {
                        ctrl.SelectedValue = Mapper2.GetStr(Info.GetValue(model, null));
                    }
                }
                if (item is DatePicker)
                {
                    var ctrl = item as DatePicker;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("date", ""));
                    if (Info != null)
                    {
                        ctrl.Text = (DateTime.Parse(Mapper2.GetStr(Info.GetValue(model, null)))).ToString("yyyy-MM-dd");
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
        public static void BindModel<T>(Control item, IDbAction<T> model) where T : IDbAction<T>, IEntityBase, new()
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
                        if (!(item is HiddenField)
                            && ((Info.Name == "Id" && ctrl.Text.ToGuid() != Guid.Empty)
                            || Info.Name != "Id"))
                        {
                            Info.SetValue(model, Mapper2.GetType(Info, ctrl.Text), null);
                        }
                    }
                }
                if (item is Label)
                {
                    var ctrl = (Label)item;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("lbl", ""));
                    if (Info != null && ctrl.Text != "" &&
                        !(Info.Name == "Id" && String.IsNullOrWhiteSpace(ctrl.Text)))
                    {
                        Info.SetValue(model, Mapper2.GetType(Info, ctrl.Text), null);
                    }
                }
                if (item is DatePicker)
                {
                    var ctrl = (DatePicker)item;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("date", ""));
                    if (Info != null && ctrl.Text != "")
                    {
                        Info.SetValue(model, Mapper2.GetType(Info, ctrl.SelectedDate.Value.ToShortDateString()), null);
                    }
                }
                if (item is DropDownList)
                {
                    var ctrl = (DropDownList)item;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("drop", ""));
                    if (Info != null && !String.IsNullOrWhiteSpace(ctrl.SelectedValue))
                    {
                        Info.SetValue(model, Mapper2.GetType(Info, ctrl.SelectedValue), null);
                    }
                }
                if (item is CheckBox)
                {
                    var ctrl = (CheckBox)item;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("chk", ""));
                    if (Info != null)
                    {
                        if (ctrl.Checked)
                        {
                            Info.SetValue(model,
                                Info.PropertyType == typeof(Boolean)
                                    ? Mapper2.GetType(Info, "True")
                                    : Mapper2.GetType(Info, "1"), null);
                        }
                        else
                        {
                            Info.SetValue(model,
                                Info.PropertyType == typeof(Boolean)
                                    ? Mapper2.GetType(Info, "False")
                                    : Mapper2.GetType(Info, "0"), null);
                        }
                    }
                }
                if (item is CheckBoxList)
                {
                    var ctrl = (CheckBoxList)item;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("chk", ""));
                    if (Info != null)
                    {
                        Info.SetValue(model, Mapper2.GetType(Info, PageTools.CheckBoxList(ctrl, "")), null);
                    }
                }
                if (item is RadioButton)
                {
                    var ctrl = (RadioButton)item;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("radio", ""));
                    if (Info != null)
                    {
                        Info.SetValue(model, ctrl.Checked ? Mapper2.GetType(Info, "1") : Mapper2.GetType(Info, "0"),
                            null);
                    }
                }
                if (item is RadioButtonList)
                {
                    var ctrl = (RadioButtonList)item;
                    Info = model.GetType().GetProperty(ctrl.ID.Replace("radio", ""));
                    if (Info != null)
                    {
                        Info.SetValue(model, Mapper2.GetType(Info, ctrl.SelectedValue), null);
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
            var temp = "";
            if (chklCheck == "")
            {
                for (var i = 0; i < chkl.Items.Count; i++)
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
                for (var j = 0; j < chkl.Items.Count; j++)
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
                    var ctrl = item as ControlBase;
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
                    var ctrl = item as RealTextField;
                    ctrl.Text = "";
                    ctrl.ClearInvalid();
                }
                if (item is Label)
                {
                    var ctrl = item as Label;
                    ctrl.Text = "";
                    ctrl.ClearInvalid();
                }
                if (item is DropDownList)
                {
                    var ctrl = item as DropDownList;
                    ctrl.SelectedIndex = 0;
                    ctrl.ClearInvalid();
                }
                if (item is Tree)
                {
                    var ctrl = item as Tree;
                    ctrl.Nodes.Clear();
                }
                if (item is CheckBox)
                {
                    var ctrl = item as CheckBox;
                    ctrl.Checked = false;
                    ctrl.ClearInvalid();
                }
                if (item is CheckBoxList)
                {
                    var ctrl = item as CheckBoxList;
                    foreach (var i in ctrl.Items)
                    {
                        i.Selected = false;
                    }
                    ctrl.ClearInvalid();
                }
                if (item is RadioButton)
                {
                    var ctrl = item as RadioButton;
                    ctrl.Checked = false;
                    ctrl.ClearInvalid();
                }
                if (item is RadioButtonList)
                {
                    var ctrl = item as RadioButtonList;
                    foreach (var i in ctrl.Items)
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
                    var ctrl = item as Grid;
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
        public static void BindDropdownList(List<Kuple<Guid, string>> items, DropDownList drop, string selectItem,
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
            var s = String.Empty;
            var random = new Random();
            for (var i = 0; i < 6; i++)
            {
                s += random.Next(10).ToString();
            }
            return s;
        }
    }
}