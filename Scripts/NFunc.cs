using Godot;
using System;

public class NFunc : Node
{
    public String Choise(String []Variant, float []Weights){
        float Mass = 0f;
        foreach(int i in GD.Range(Weights.Length)){
            Mass += Weights[i];
        }
        float _Variant = (float)GD.RandRange(0, Mass);
        float _Tmp = 0;
        foreach(int i in GD.Range(Weights.Length)){
            _Tmp += Weights[i];
            if(_Tmp >= _Variant){
                return Variant[i];
            }
        }
        return Variant[0];
    }
}
