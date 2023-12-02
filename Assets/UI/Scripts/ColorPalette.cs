using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalette
{
    public enum ColorLM {
        WHITE,
        BLACK,
        RED,
        GREEN,
        BLUE,
        YELLOW,
        PURPLE,
    }

    public static Color getColor(ColorLM color){
        Color newColor = new Color(0,0,0); 

        switch(color){
            case ColorLM.WHITE:
                newColor = new Color(1,1,1);
                break;
            case ColorLM.BLACK:
                newColor = new Color(0,0,0);
                break;
            case ColorLM.RED:
                newColor = new Color(255 / 255.0f, 89 / 255.0f, 94 / 255.0f);
                break;
            case ColorLM.GREEN:
                newColor = new Color(167 / 255.0f, 201 / 255.0f, 87 / 255.0f);
                break; 
            case ColorLM.BLUE:
                newColor = new Color(25 / 255.0f, 130 / 255.0f, 196 / 255.0f);
                break;
            case ColorLM.YELLOW:
                newColor = new Color(255 / 255.0f, 202 / 255.0f, 58 / 255.0f);
                break;
            case ColorLM.PURPLE:
                newColor = new Color(106 / 255.0f, 76 / 255.0f, 147 / 255.0f);
                break;
            default:
                return new Color(0,0,0);
        }
        return newColor; 
    }
}
