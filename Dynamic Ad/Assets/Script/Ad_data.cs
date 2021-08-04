using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ad_data 
{
    public List<ad_layers> layers;
}

[Serializable]
public class ad_layers
{
    public string type=null;
    public string path = null;
    public Texture2D texture = null;
    public List<layer_placement> placement;
    public List<layer_operations> operations;
}

[Serializable]
public class layer_placement
{
    public layer_placement_position position;
}

[Serializable]
public class layer_operations
{
    public string name;
    public string argument;
}

[Serializable]
public class layer_placement_position
{
    public int x = 0;
    public int y = 0;
    public int width = 0;
    public int height = 0;
}
