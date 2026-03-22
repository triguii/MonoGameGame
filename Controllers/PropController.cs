using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Timers;
using MonoGameLibrary;
using PilotGame.Controllers;
using PilotGame.GameObjects;
using PilotGame.GameObjects.Props;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PilotGame.Controllers;

public static class PropController
{

    
    public static Texture2DAtlas propsAtlas = Core.Content.Load<Texture2DAtlas>("maps/props/props-atlas");
    public static SpriteSheet propsSheet = new SpriteSheet("maps/props/props-atlas-texture", propsAtlas);


    public static List<Prop> InitialitzeMap(String mapDirectory)
    {
        List<Prop> propsList = new List<Prop>();
        string filePath = Path.Combine(Core.Content.RootDirectory, mapDirectory + ".xml");


        using (Stream stream = TitleContainer.OpenStream(filePath))
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {

                XDocument doc = XDocument.Load(reader);
                XElement root = doc.Root;

                var props = root.Element("Props")?.Elements("Prop");

                if (props != null)
                {
                    foreach (var prop in props)
                    {
                        string name = prop.Attribute("name")?.Value;
                        int x = int.Parse(prop.Attribute("x")?.Value ?? "0");
                        int y = int.Parse(prop.Attribute("y")?.Value ?? "0");


                        if (!string.IsNullOrEmpty(name))
                        {
                            //Crear una instancia de cada prop en funcion del nombre en el archivo XML, asumiendo que el nombre coincide con el nombre de la clase del prop

                            Type propType = Type.GetType("PilotGame.GameObjects.Props." + name);
                            object instance = Activator.CreateInstance(propType);

                            dynamic propInstance = instance;
                            propInstance.Initialize(new Vector2(x, y));
                            propsList.Add(propInstance);
                        }
                    }
                }

            }
        }

        return propsList;
    }

    public static void setAnimation(string animationName, Action<SpriteSheetAnimationBuilder> animBuilder)
    {
        try
        {
            propsSheet.DefineAnimation(animationName, animBuilder);

        }
        catch (ArgumentException ex)
        {
            return;
        }
        
    }

}
