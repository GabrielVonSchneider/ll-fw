require 'childwindow'

local function faction_list_item(ids, color)
{
	local li = NewObject("ListItem")
	// Border
	li.Border = NewObject("UiRenderable")
	local wire = NewObject("DisplayWireBorder")
	wire.Color = GetColor("text")
	li.Border.AddElement(wire)
	li.HoverBorder = NewObject("UiRenderable")
	wire = NewObject("DisplayWireBorder")
	wire.Color = GetColor("slow_blue_yellow")
	li.HoverBorder.AddElement(wire)
	li.SelectedBorder = NewObject("UiRenderable")
	wire = NewObject("DisplayWireBorder")
	wire.Color = GetColor("yellow")
	li.SelectedBorder.AddElement(wire)
	//Contents
	li.ItemA = NewObject("Panel")
	local ta = NewObject("TextBlock")
	ta.TextSize = 9
	ta.HorizontalAlignment = HorizontalAlignment.Center
	ta.VerticalAlignment = VerticalAlignment.Top
	ta.TextColor = GetColor(color)
	ta.TextShadow = GetColor("black")
	ta.Fill = true
	ta.Strid = ids
	li.ItemA.Children.Add(ta)
	return li;
}

class pickfaction : pickfaction_Designer with ChildWindow
{
    pickfaction()
    {
        base();
        this.ChildWindowInit();
        this.Elements.close.OnClick(() => this.Close());
		this.Elements.ok.Enabled = false;
		this.Elements.ok.OnClick(() => this.PickFaction());
		this.OnChildOpen();
    }

	OnChildOpen()
	{
		local e = this.Elements;
		this.Factions = Game.Arena.GetAvailableFactions();
		e.factions.Children.Clear();
		for (f in this.Factions) {
			e.factions.Children.Add(faction_list_item(f.IdsName, f.Color));
		}
		e.factions.OnSelectedIndexChanged(() => this.SelectedFactionChanged());
	}
	
	SelectedFactionChanged()
	{
		local e = this.Elements;
		faction_selected = e.factions.SelectedIndex >= 0
		e.ok.Enabled = faction_selected;
	}
	
	PickFaction()
	{
		this.Close();
		Game.Arena.PickFaction(this.Elements.factions.SelectedIndex);
	}
}

