﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OSAE;

public partial class readers : System.Web.UI.Page
{
    
    public void RaisePostBackEvent(string eventArgument)
    {
        string[] args = eventArgument.Split('_');

        if (args[0] == "gvReaders")
        {
            hdnSelectedReadersRow.Text = args[1];
            hdnSelectedReadersName.Text = gvReaders.DataKeys[Int32.Parse(hdnSelectedReadersRow.Text)]["object_name"].ToString();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        loadReaders();

        if (!this.IsPostBack)
        {
            loadObjects();
        }

        if (hdnSelectedReadersRow.Text != "")
        { }

    
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

        if (hdnSelectedReadersLastRow.Text != hdnSelectedReadersRow.Text)
        {
            string selectedObject = gvReaders.DataKeys[Int32.Parse(hdnSelectedReadersRow.Text)]["object_name"].ToString();
            if (ddlObjects.Items.FindByText(selectedObject) != null)
            {
                ddlObjects.SelectedItem.Selected = false;
                ddlObjects.Items.FindByText(selectedObject).Selected = true;
                loadProperties();
                string selectedProperty = gvReaders.DataKeys[Int32.Parse(hdnSelectedReadersRow.Text)]["property_name"].ToString();
                if (ddlProperties.Items.FindByText(selectedProperty) != null)
                {
                    ddlProperties.SelectedItem.Selected = false;
                    ddlProperties.Items.FindByText(selectedProperty).Selected = true;
                }
            }
            txtURL.Text = gvReaders.DataKeys[Int32.Parse(hdnSelectedReadersRow.Text)]["URL"].ToString();
            txtPrefix.Text = gvReaders.DataKeys[Int32.Parse(hdnSelectedReadersRow.Text)]["search_prefix"].ToString();
            txtPrefixOffset.Text = gvReaders.DataKeys[Int32.Parse(hdnSelectedReadersRow.Text)]["search_prefix_offset"].ToString();
            txtSuffix.Text = gvReaders.DataKeys[Int32.Parse(hdnSelectedReadersRow.Text)]["search_suffix"].ToString();
            txtInterval.Text = gvReaders.DataKeys[Int32.Parse(hdnSelectedReadersRow.Text)]["update_interval"].ToString();
            hdnSelectedReadersLastRow.Text = hdnSelectedReadersRow.Text;
        }

        if (hdnSelectedReadersRow.Text != "")
        {
            gvReaders.Rows[Int32.Parse(hdnSelectedReadersRow.Text)].Attributes.Remove("onmouseout");
            gvReaders.Rows[Int32.Parse(hdnSelectedReadersRow.Text)].Style.Add("background", "lightblue");
        }
    }

    protected void gvReaders_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            e.Row.Attributes.Add("onmouseover", "this.style.cursor='hand';this.style.background='lightblue';");
            if (e.Row.RowState == DataControlRowState.Alternate)
                e.Row.Attributes.Add("onmouseout", "this.style.background='#fcfcfc url(Images/grd_alt.png) repeat-x top';");
            else
                e.Row.Attributes.Add("onmouseout", "this.style.background='none';");
            e.Row.Attributes.Add("onclick", ClientScript.GetPostBackClientHyperlink(this, "gvReaders_" + e.Row.RowIndex.ToString()));
        }

    }


    private void loadReaders()
    {
        gvReaders.DataSource = OSAESql.RunSQL("SELECT object_name,property_name,object_property_scraper_id,object_id,object_property_id,URL,search_prefix,search_prefix_offset,search_suffix,update_interval FROM osae_v_object_property_scraper ORDER BY object_name,property_name");
        gvReaders.DataBind();
       
    }

    private void loadObjects()
    {
        ddlObjects.DataSource = OSAESql.RunSQL("SELECT DISTINCT(object_name) as object_name, object_id FROM osae_v_object_property ORDER BY object_name");
        ddlObjects.DataBind();
    }

    private void loadProperties()
    {
        ddlProperties.DataSource = OSAESql.RunSQL("SELECT DISTINCT(property_name) as property_name,property_id FROM osae_v_object_property WHERE object_id=" + ddlObjects.SelectedValue + " ORDER BY property_name");
        ddlProperties.DataBind();
    }



    protected void btnReaderAdd_Click(object sender, EventArgs e)
    {
        OSAEObjectPropertyManager.ObjectPropertyScraperAdd(ddlObjects.SelectedItem.Text, ddlProperties.SelectedItem.Text, txtURL.Text, txtPrefix.Text, Convert.ToInt16(txtPrefixOffset.Text), txtSuffix.Text, txtInterval.Text);
        loadReaders();
    }
    protected void btnReaderUpdate_Click(object sender, EventArgs e)
    {
        OSAEObjectPropertyManager.ObjectPropertyScraperUpdate(Convert.ToInt16(gvReaders.DataKeys[Int32.Parse(hdnSelectedReadersRow.Text)]["object_property_scraper_id"].ToString()), ddlObjects.SelectedItem.Text, ddlProperties.SelectedItem.Text, txtURL.Text, txtPrefix.Text, Convert.ToInt16(txtPrefixOffset.Text), txtSuffix.Text, txtInterval.Text);
        loadReaders();
    }
    protected void btnReaderDelete_Click(object sender, EventArgs e)
    {
        OSAEObjectPropertyManager.ObjectPropertyScraperDelete(Convert.ToInt16(gvReaders.DataKeys[Int32.Parse(hdnSelectedReadersRow.Text)]["object_property_scraper_id"].ToString()));
     }
    protected void ddlObjects_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadProperties();
    }
}