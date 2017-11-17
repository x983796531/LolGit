Shader "Custom/WarFog" {  
    Properties {  
        _MainTex ("_MainTex", 2D) = "white" {}  
		_MaskTex ("_MaskTex", 2D) = "white" {}
    }  
    SubShader {  
        Pass {  
            CGPROGRAM  
            #pragma vertex vert_img  
            #pragma fragment frag  
               
            #include "UnityCG.cginc"  
               
            uniform sampler2D _MainTex;  
			uniform sampler2D _MaskTex; 
               
            fixed4 frag(v2f_img i) : COLOR  
            {  
                fixed4 renderTex = tex2D(_MainTex, i.uv);  
                 fixed4 renderTex1 = tex2D(_MaskTex, i.uv); 
                fixed4 finalColor;
				if(renderTex1.r<.3){
					finalColor = renderTex1.rgba;
				}else{
					finalColor = renderTex.rgba;
				} 
                   
                return finalColor;  
            }  
               
            ENDCG  
        }  
    }  
    FallBack "Diffuse"  
}
