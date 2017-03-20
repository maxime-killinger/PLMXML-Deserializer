using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace PLMXMLParser
{
    public class Exportation
    {
        static List<Instance> Instances;
        static List<string> IdSousRefNonExistantes = new List<string>();
        static bool DisplayRepresentation = false;

        static void Main()
        {

        }

		public static int Export(string str)
        {
            List<string> Data = new List<string>();
            Data.Add("PartNumber");
            Data.Add("ProjectName");
            Data.Add("Nomenclature");

            Instances = new List<Instance>();

            if (!File.Exists(str))
            {
                return (-2);
            }
            string contenu = File.ReadAllText(str);
            StringReader sr = new StringReader(contenu);

            try
            {
                using (XmlReader reader = XmlReader.Create(sr))
                {

                    while (reader.ReadToFollowing("Instance"))
                    {
                        Instance i = new Instance();

                        reader.MoveToAttribute(0);
                        if (reader.HasValue)
                            i.Id = reader.Value;
                        else
                            throw new ApplicationException("Pas d'ID pour l'instance N° " + Instances.Count.ToString());

						reader.MoveToAttribute(1);
                        if (reader.HasValue)
                            i.Partref = reader.Value;
                        else
                            throw new ApplicationException("Pas de référence de pièce pour l'instance N° " + Instances.Count.ToString());
                        
                        i.UserValues = GetUserValuesFromUserData(reader, Data);

                        if (reader.ReadToFollowing("Part"))
                        {
                            Part p = new Part();
                            Representation r = new Representation();

                            reader.MoveToAttribute(0);
                            if (reader.HasValue)
                                p.Id = reader.Value;
                            else
                                throw new ApplicationException("Pas d'ID pour l'instance N° " + Instances.Count.ToString());
                        
                            reader.MoveToAttribute(1);
                            if (reader.HasValue)
                                p.Name = reader.Value;
                        
                            if (reader.MoveToAttribute("instanceRefs"))
                            {
                                if (reader.HasValue)
                                    p.ListeIdSousRefs = reader.Value.Split(new char[] { ' ' }).ToList<String>();
                            }

                            p.UserValues = GetUserValuesFromUserData(reader, Data);

                            if (DisplayRepresentation)
                            {
                                if (reader.ReadToNextSibling("Representation"))
                                {
                                    reader.MoveToAttribute(0);
                                    if (reader.HasValue)
                                        r.Id = reader.Value;

                                    reader.MoveToAttribute(1);
                                    if (reader.HasValue)
                                        r.Load = reader.Value;

                                    r.UserValues = GetUserValuesFromUserData(reader, Data);
                                }
                                p.Representations.Add(r);
                            }

                            i.Piece = p;
                            if (i.Partref != p.Id)
                            {
                                throw new ApplicationException("L'ID de la pièce pour l'instance N° " + i.Id + " ne correspond pas à la pièce");
                            }


                        }
                        else
                        {
                            throw new ApplicationException("Pas de pièce pour l'instance N° " + Instances.Count.ToString());

                        }

                        Instances.Add(i);
                    }
                    reader.Close();
                }
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

			foreach (Instance i in Instances)
            {
                foreach (String idSousref in i.Piece.ListeIdSousRefs)
                {
                    Instance Sousref = Instances.Where(inst => inst.Id == idSousref).FirstOrDefault();
                    if (Sousref != null)
                        i.Piece.Instances.Add(Sousref);
                    else
                        IdSousRefNonExistantes.Add(idSousref);

                }

            }

            SaveTab(str);

            if (IdSousRefNonExistantes.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Liste des sous-références non présentes:");
                foreach (string id in IdSousRefNonExistantes)
                {
                    Console.WriteLine(id);
                }
            }
            return (0);
        }

        private static Dictionary<string, string> GetUserValuesFromUserData(XmlReader reader, List<string> Data)
        {
            Dictionary<string, string> userValues = new Dictionary<string, string>();

            if (reader.ReadToFollowing("UserData"))
            {
                if (reader.ReadToDescendant("UserValue"))
                {
                    do
                    {
                        UserValue temp = GetUserValueFromReader(reader);
                        if (CompareTitle(temp.Title, Data))
                            userValues.Add(temp.Title, temp.Value);
                    } while (reader.ReadToNextSibling("UserValue"));
                }
            }

            return userValues;
        }

        private static bool CompareTitle(string Title, List<string> Data)
        {
            foreach (string str in Data)
            {
                if (Title == str)
                    return (true);
            }
            return (false);
        }

        private static UserValue GetUserValueFromReader(XmlReader reader)
        {
            UserValue userValue = new UserValue();
            if (reader.MoveToFirstAttribute())
            {
                userValue.Title = reader.Value;
            }
            if (reader.MoveToNextAttribute())
            {
                userValue.Value = reader.Value;
            }
            return userValue;
        }

        static void SaveTab(string name)
        {
            if (!Directory.Exists(@"C:\Export"))
            {
                Directory.CreateDirectory(@"C:\Export");
            }
            string str = "Compose;ProjectName;Nomenclature;Composant;ProjectName;Nomenclature;\n";

            foreach (Instance i in Instances)
            {
                int j = 1;
                do
                {
                    if (i.Piece.UserValues.ContainsKey("PartNumber"))
                        str = str + "\"" + i.Piece.UserValues["PartNumber"] + "\"";
                    str = str + ";";
                    if (i.Piece.UserValues.ContainsKey("ProjectName"))
                        str = str + "\"" + i.Piece.UserValues["ProjectName"] + "\"";
                    str = str + ";";
                    if (i.Piece.UserValues.ContainsKey("Nomenclature"))
                        str = str + "\"" + i.Piece.UserValues["Nomenclature"] + "\"";
                    str = str + ";";
                    if (i.Piece.ListeIdSousRefs.Count > 0)
                    {
                        foreach (Instance y in Instances)
                        {
                            if (i.Piece.ListeIdSousRefs[j - 1] == y.Id)
                            {
                                if (y.Piece.UserValues.ContainsKey("PartNumber"))
                                    str = str + "\"" + y.Piece.UserValues["PartNumber"] + "\"";
                                str = str + ";";
                                if (y.Piece.UserValues.ContainsKey("ProjectName"))
                                    str = str + "\"" + y.Piece.UserValues["ProjectName"] + "\"";
                                str = str + ";";
                                if (y.Piece.UserValues.ContainsKey("Nomenclature"))
                                    str = str + "\"" + y.Piece.UserValues["Nomenclature"] + "\"";
                                str = str + ";";
                                break;
                            }
                        }
                    }
                    else
                    {
                        str = str + ";;;";
                    }
                    str = str + "\n";
                } while (j++ < i.Piece.ListeIdSousRefs.Count);
            }
            System.IO.File.WriteAllText(@"C:\Export\" + name.Split(new char[] { '\\' }).Last<string>() + ".csv", str);
        }
    }
}
