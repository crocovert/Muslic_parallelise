using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Globalization;
using System.Threading;

namespace Muslic
{
    partial class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() > 3)
            {
                // Appeler la version parallelisée
                Program.ExecuteParallelAffectation_OD(args[0], args[1], args[2], args[3], args.Count() > 4 ? args[4] : null);
            }
        }
        // Point d'entree appele par Main : meme parametres que affectation_tc (parallelisee sur les OD)
        public static void ExecuteParallelAffectation_OD(string nom_reseau, string nom_matrice, string nom_sortie, string nom_parametres, string nom_penalites)
        {
            affectation_tc(nom_reseau, nom_matrice, nom_sortie, nom_parametres, nom_penalites);
        }

        public static void Ecrit_parametres(Param_affectation_horaire parametres, string nom_fichier_ini)
        {

            System.IO.StreamWriter fich_ini = new System.IO.StreamWriter(nom_fichier_ini, false, System.Text.Encoding.UTF8);
            if (parametres.sortie_stops == true)
            {
                parametres.sortie_temps += 10;
            }
            ;
            String texte = parametres.algorithme + ";algorithm" +
                "\n" + parametres.demitours + ";prohibited U-turns" +
                "\n" + parametres.max_nb_buckets + ";max buckets" +
                "\n" + parametres.nb_jours + ";number of days" +
                "\n" + parametres.nom_matrice + ";matrix file" +
                "\n" + parametres.nom_penalites + ";turns and transfers file" +
                "\n" + parametres.nom_reseau + ";network file" +
                "\n" + parametres.nom_sortie + ";generic output file" +
                "\n" + parametres.param_dijkstra + ";algorithm parameter+" +
                "\n" + parametres.pu + ";algorithm power" +
                "\n" + parametres.sortie_chemins + ";output paths" +
                "\n" + parametres.sortie_services + ";output services" +
                "\n" + parametres.sortie_temps + ";output travel times" +
                "\n" + parametres.sortie_turns + ";output turns and transfers" +
                "\n" + parametres.texte_cboa + ";boarding weight" +
                "\n" + parametres.texte_cmap + ";individual mode weight" +
                "\n" + parametres.texte_coef_tmap + ";individual travel time factor" +
                "\n" + parametres.texte_cveh + ";in-vehicle time weight" +
                "\n" + parametres.texte_cwait + ";wait time weight" +
                "\n" + parametres.texte_tboa + ";min transfer time" +

                "\n" + parametres.texte_tboa_max + ";max transfer time" +
                "\n" + parametres.tmapmax + ";max individual travel time" +
                "\n" + parametres.texte_toll + ";toll weight" +
                "\n" + parametres.texte_filtre_sortie + ";output filter types" +
                "\n" + parametres.temps_max + ";max travel cost" +
                "\n" + parametres.sortie_noeuds + ";output nodes" +
                "\n" + parametres.sortie_isoles + ";output isolated links" +
                (parametres.avance_retard ? "\n" + parametres.pond_avance + ";early departure/arrival weight\n" + parametres.pond_retard + ";late departure/arrival weight" : "");

            fich_ini.WriteLine(texte);
            fich_ini.Close();






        }



        public static Param_affectation_horaire lit_parametres(string nom_parametres)
        {

            Param_affectation_horaire aff_hor = new Param_affectation_horaire();
            if (System.IO.File.Exists(nom_parametres) == true)
            {
                System.IO.StreamReader fich_ini = new System.IO.StreamReader(nom_parametres);

                aff_hor.algorithme = int.Parse(fich_ini.ReadLine().Split(';')[0]);
                //                Console.WriteLine(aff_hor.algorithme);
                aff_hor.demitours = bool.Parse(fich_ini.ReadLine().Split(';')[0]);
                //Console.WriteLine(aff_hor.demitours);
                aff_hor.max_nb_buckets = int.Parse(fich_ini.ReadLine().Split(';')[0]);
                //Console.WriteLine(aff_hor.max_nb_buckets);
                aff_hor.nb_jours = int.Parse(fich_ini.ReadLine().Split(';')[0]);
                //Console.WriteLine(aff_hor.nb_jours);
                aff_hor.nom_matrice = fich_ini.ReadLine().Split(';')[0];
                //Console.WriteLine(aff_hor.nom_matrice);
                aff_hor.nom_penalites = fich_ini.ReadLine().Split(';')[0];
                //Console.WriteLine(aff_hor.nom_penalites);
                aff_hor.nom_reseau = fich_ini.ReadLine().Split(';')[0];
                //Console.WriteLine(aff_hor.nom_reseau);
                aff_hor.nom_sortie = fich_ini.ReadLine().Split(';')[0];
                //Console.WriteLine(aff_hor.nom_sortie);
                aff_hor.param_dijkstra = int.Parse(fich_ini.ReadLine().Split(';')[0]);
                //Console.WriteLine(aff_hor.param_dijkstra);
                aff_hor.pu = float.Parse(fich_ini.ReadLine().Split(';')[0]);
                //Console.WriteLine(aff_hor.pu);
                aff_hor.sortie_chemins = bool.Parse(fich_ini.ReadLine().Split(';')[0]);
                //Console.WriteLine(aff_hor.sortie_chemins);
                aff_hor.sortie_services = bool.Parse(fich_ini.ReadLine().Split(';')[0]);
                //Console.WriteLine(aff_hor.sortie_services);
                aff_hor.sortie_temps = int.Parse(fich_ini.ReadLine().Split(';')[0]);
                if (aff_hor.sortie_temps >= 10)
                {
                    aff_hor.sortie_stops = true;
                    aff_hor.sortie_temps += -10;
                }
                else
                {
                    aff_hor.sortie_stops = false;
                }
                //Console.WriteLine(aff_hor.sortie_temps);
                aff_hor.sortie_turns = bool.Parse(fich_ini.ReadLine().Split(';')[0]);
                //Console.WriteLine(aff_hor.sortie_turns);
                aff_hor.texte_cboa = fich_ini.ReadLine().Split(';')[0];
                //Console.WriteLine(aff_hor.texte_cboa);
                aff_hor.texte_cmap = fich_ini.ReadLine().Split(';')[0];
                //Console.WriteLine(aff_hor.texte_cmap);
                aff_hor.texte_coef_tmap = fich_ini.ReadLine().Split(';')[0];
                //Console.WriteLine(aff_hor.texte_coef_tmap);
                aff_hor.texte_cveh = fich_ini.ReadLine().Split(';')[0];
                //Console.WriteLine(aff_hor.texte_cveh);
                aff_hor.texte_cwait = fich_ini.ReadLine().Split(';')[0];
                //Console.WriteLine(aff_hor.texte_cwait);
                aff_hor.texte_tboa = fich_ini.ReadLine().Split(';')[0];
                //Console.WriteLine(aff_hor.texte_tboa);
                aff_hor.texte_tboa_max = fich_ini.ReadLine().Split(';')[0];
                //Console.WriteLine(aff_hor.texte_tboa_max);
                //aff_hor.sortie_noeuds = bool.Parse(fich_ini.ReadLine().Split(';')[0]);
                //Console.WriteLine(aff_hor.texte_tboa_max);
                // aff_hor.sortie_isoles = bool.Parse(fich_ini.ReadLine().Split(';')[0]);
                //Console.WriteLine(aff_hor.texte_tboa_max);

                if (fich_ini.EndOfStream == false)
                {
                    if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".")
                    {
                        aff_hor.tmapmax = float.Parse(fich_ini.ReadLine().Replace(",", ".").Split(';')[0]);
                    }
                    else
                    {
                        aff_hor.tmapmax = float.Parse(fich_ini.ReadLine().Replace(".", ",").Split(';')[0]);
                    }

                    //  Console.WriteLine(aff_hor.tmapmax);
                }
                if (fich_ini.EndOfStream == false)
                {
                    aff_hor.texte_toll = fich_ini.ReadLine().Split(';')[0];
                    //Console.WriteLine(aff_hor.texte_toll);
                }

                if (fich_ini.EndOfStream == false)
                {
                    aff_hor.texte_filtre_sortie = fich_ini.ReadLine().Split(';')[0];
                    //Console.WriteLine(aff_hor.texte_filtre_sortie);
                }


                if (fich_ini.EndOfStream == false)
                {
                    if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".")
                    {
                        aff_hor.temps_max = float.Parse(fich_ini.ReadLine().Replace(",", ".").Split(';')[0]);
                    }
                    else
                    {
                        aff_hor.temps_max = float.Parse(fich_ini.ReadLine().Replace(".", ",").Split(';')[0]);
                    }
                    //Console.WriteLine(aff_hor.temps_max);
                }
                if (fich_ini.EndOfStream == false)
                {
                    aff_hor.sortie_noeuds = bool.Parse(fich_ini.ReadLine().Split(';')[0]);
                    //Console.WriteLine(aff_hor.sortie_noeuds);
                }
                if (fich_ini.EndOfStream == false)
                {
                    aff_hor.sortie_isoles = bool.Parse(fich_ini.ReadLine().Split(';')[0]);
                    //Console.WriteLine(aff_hor.sortie_isoles);
                }
                // ponderations optionnelles : depart/arrivee en avance, puis depart/arrivee en retard
                if (fich_ini.EndOfStream == false)
                {
                    aff_hor.pond_avance = LitFlottantCulture(fich_ini.ReadLine().Split(';')[0]);
                    aff_hor.avance_retard = true;
                }
                if (fich_ini.EndOfStream == false)
                {
                    aff_hor.pond_retard = LitFlottantCulture(fich_ini.ReadLine().Split(';')[0]);
                    aff_hor.avance_retard = true;
                }
                fich_ini.Close();
            }
            aff_hor.test_OK = true;
            return aff_hor;
        }

        public static void affectation_tc(string nom_reseau, string nom_matrice, string nom_sortie, string nom_parametres, string nom_penalites)
        {
            int i, j;

            HashSet<String> types = new HashSet<string>();
            Dictionary<Turn, float> turns = new Dictionary<Turn, float>();
            Dictionary<Turn, float> turns_toll = new Dictionary<Turn, float>(); // peage associe a un virage (7e colonne du fichier de penalites)
            Dictionary<Turn, float> turns_cost = new Dictionary<Turn, float>(); // cout additionnel associe a un virage / sous-chemin
            List<SousChemin> pats = new List<SousChemin>();                     // sous-chemins de 3 arcs ou plus
            Dictionary<Turn, float> transfers = new Dictionary<Turn, float>();
            Dictionary<Link_num, int> link_id = new Dictionary<Link_num, int>();
            etude projet = new etude();
            Param_affectation_horaire aff_hor = lit_parametres(nom_parametres);
            //string nom_reseau = aff_hor.nom_reseau;
            //string nom_matrice = aff_hor.nom_matrice;
            //string nom_penalites = aff_hor.nom_penalites;
            //Console.WriteLine(nom_sortie);
            projet.param_affectation_horaire = aff_hor;
            aff_hor.nom_sortie = nom_sortie;
            aff_hor.nom_reseau = nom_reseau;
            aff_hor.nom_matrice = nom_matrice;
            aff_hor.nom_penalites = nom_penalites;
            if (System.IO.File.Exists(nom_parametres) == true)
            {
                if (System.IO.File.Exists(nom_reseau) == true && System.IO.File.Exists(nom_matrice) == true && projet.param_affectation_horaire.test_OK == true)
                {

                    string[] param = { ";" };
                    if (projet.reseaux.Count > 0)
                    {
                        projet.reseaux.RemoveAt(projet.reseaux.Count - 1);
                    }
                    projet.reseaux.Add(new network());
                    int num_res;


                    string chaine;
                    string[] ch;

                    projet.reseau_actif = projet.reseaux.Count - 1;
                    num_res = projet.reseaux.Count - 1;
                    //openFileDialog1.ShowDialog();

                    string carte = "t links";

                    int avancement = 0;
                    int ctop = Console.CursorTop;
                    int cleft = Console.CursorLeft;
                    Console.SetCursorPosition(cleft, ctop);
                    Console.Write("Network import:" + avancement + "%");
                    //            System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator = ".";
                    System.IO.FileStream flux_reseau;

                    flux_reseau = new System.IO.FileStream(nom_reseau, System.IO.FileMode.Open, FileAccess.Read, System.IO.FileShare.Read);
                    System.IO.StreamReader fichier_reseau = new System.IO.StreamReader(flux_reseau, Encoding.UTF8);
                    //   projet.reseaux[num_res].matrices.Add(new matrix());
                    System.IO.StreamWriter fich_log = new System.IO.StreamWriter(aff_hor.nom_sortie + "_log.txt", false, System.Text.Encoding.UTF8);
                    fich_log.WriteLine("Version: Muslic " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    fich_log.WriteLine("Process start time: " + System.DateTime.Now.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
                    fich_log.WriteLine("Default parameters:");
                    fich_log.WriteLine("Minimum transfer time:" + aff_hor.texte_tboa);
                    fich_log.WriteLine("Maximum transfer time:" + aff_hor.texte_tboa_max);
                    fich_log.WriteLine("Transfer weight:" + aff_hor.texte_cboa);
                    fich_log.WriteLine("Wait weight:" + aff_hor.texte_cwait);
                    fich_log.WriteLine("Time based links weight:" + aff_hor.texte_cveh);
                    fich_log.WriteLine("Individual links weight:" + aff_hor.texte_cmap);
                    fich_log.WriteLine("Individual travel time factor:" + aff_hor.texte_coef_tmap);
                    fich_log.WriteLine("Generalized travel time maximum:" + aff_hor.temps_max);
                    fich_log.WriteLine("Individual travel time maximum:" + aff_hor.tmapmax.ToString());
                    fich_log.WriteLine("Toll weight:" + aff_hor.texte_toll.ToString());
                    fich_log.WriteLine("Number of days:" + aff_hor.nb_jours);
                    fich_log.WriteLine("Prohibited U-turns:" + aff_hor.demitours);

                    fich_log.WriteLine("Algorithm:" + aff_hor.algorithme);
                    fich_log.WriteLine("Algorithm number of buckets:" + aff_hor.max_nb_buckets);
                    fich_log.WriteLine("Algorithm scale parameter:" + aff_hor.param_dijkstra);
                    fich_log.WriteLine("Algorithm power parameter:" + aff_hor.pu);

                    fich_log.WriteLine("Output paths:" + aff_hor.sortie_chemins);
                    fich_log.WriteLine("Output times:" + aff_hor.sortie_temps);
                    fich_log.WriteLine("Output filenames:" + aff_hor.nom_sortie);
                    fich_log.WriteLine("Link type filter:" + aff_hor.texte_filtre_sortie.ToString());
                    fich_log.WriteLine("Output nodes:" + aff_hor.sortie_noeuds.ToString());
                    fich_log.WriteLine("Output isolated nodes:" + aff_hor.sortie_isoles.ToString());
                    if (aff_hor.avance_retard)
                    {
                        fich_log.WriteLine("Early departure/arrival weight:" + aff_hor.pond_avance);
                        fich_log.WriteLine("Late departure/arrival weight:" + aff_hor.pond_retard);
                    }



                    projet.reseaux[projet.reseau_actif].nom = System.IO.Path.GetFileNameWithoutExtension(nom_reseau);
                lec:
                    while (fichier_reseau.EndOfStream == false)
                    {
                    lecture:

                        chaine = fichier_reseau.ReadLine();

                        if (avancement < (int)((100 * flux_reseau.Position) / flux_reseau.Length) - 4)
                        {
                            Console.SetCursorPosition(cleft, ctop);
                            Console.Write("Network import:" + ((100 * flux_reseau.Position) / flux_reseau.Length).ToString() + "%");
                            avancement = (int)((100 * flux_reseau.Position) / flux_reseau.Length);

                        }

                        if (chaine.Trim().Length == 0) goto lec;
                        if (chaine.Substring(0, 7) == "t nodes")
                        {
                            carte = "t nodes";
                            goto lecture;
                        }
                        else if (chaine.Substring(0, 7) == "t links")
                        {
                            carte = "t links";
                            goto lecture;
                        }



                        ch = chaine.Split(param, System.StringSplitOptions.None);
                        //MessageBox.Show(carte + " " + ch[0]);
                        //if ((Convert.ToSingle(ch[4]) > projet.param_affectation_horaire.deb_per && Convert.ToSingle(ch[4]) < projet.param_affectation_horaire.fin_per) || Convert.ToSingle(ch[4])<0)
                        if (carte == "t nodes")
                        {
                            string ni = ch[0].Trim();
                            if (projet.reseaux[projet.reseau_actif].numnoeud.ContainsKey(ni) == false)
                            {
                                projet.reseaux[projet.reseau_actif].numnoeud.Add(ni, projet.reseaux[projet.reseau_actif].nodes.Count);

                                float xi, yi;
                                if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".")
                                {
                                    xi = float.Parse(ch[1].Replace(',', '.'));
                                    yi = float.Parse(ch[2].Replace(',', '.'));
                                }
                                else
                                {
                                    xi = float.Parse(ch[1].Replace('.', ','));
                                    yi = float.Parse(ch[2].Replace('.', ','));
                                }
                                node noeud = new node();
                                node nul = new node();
                                noeud.i = ni;
                                noeud.x = xi;
                                noeud.y = yi;
                                noeud.is_visible = true;
                                if (xi > projet.reseaux[num_res].xu)
                                {
                                    projet.reseaux[num_res].xu = xi;
                                }
                                if (xi < projet.reseaux[num_res].xl)
                                {
                                    projet.reseaux[num_res].xl = xi;
                                }
                                if (yi > projet.reseaux[num_res].yu)
                                {

                                    projet.reseaux[num_res].yu = yi;

                                }
                                if (yi < projet.reseaux[num_res].yl)
                                {
                                    projet.reseaux[num_res].yl = yi;
                                }


                                if (ch.Length > 3)
                                {
                                    noeud.texte = ch[3];
                                }


                                projet.reseaux[projet.reseau_actif].nodes.Add(noeud);
                            }

                        }
                        else if (carte == "t links")
                        {
                            node nul = new node();
                            node nodei = new node();
                            node nodej = new node();
                            Link_num num_link = new Link_num();

                            string ni = ch[0].Trim();
                            int line;
                            nodei.i = ni;
                            /*while (projet.reseaux[projet.reseau_actif].nodes.Count < ni + 1)
                            {
                                projet.reseaux[projet.reseau_actif].nodes.Add(nul);
                            }
                            //projet.reseaux[projet.reseau_actif].numnoeud.Add(ni, projet.reseaux[projet.reseau_actif].nodes.Count);*/
                            int value;
                            if (projet.reseaux[projet.reseau_actif].numnoeud.TryGetValue(ni, out value) == false)
                            {
                                projet.reseaux[projet.reseau_actif].numnoeud.Add(ni, projet.reseaux[projet.reseau_actif].nodes.Count);
                                projet.reseaux[projet.reseau_actif].nodes.Add(nodei);
                            }
                            /*if (projet.reseaux[projet.reseau_actif].nodes[ni].i == 0)
                            {
                                projet.reseaux[projet.reseau_actif].nodes[ni] = nodei;
                            }*/

                            string nj = ch[1].Trim();

                            nodej.i = nj;
                            /*while (projet.reseaux[projet.reseau_actif].nodes.Count < nj + 1)
                            {
                                projet.reseaux[projet.reseau_actif].nodes.Add(nul);
                            }
                            if (projet.reseaux[projet.reseau_actif].nodes[nj].i == 0)
                            {
                                projet.reseaux[projet.reseau_actif].nodes[nj] = nodej;
                            }*/
                            //  MessageBox.Show(projet.reseaux[projet.reseau_actif].numnoeud.TryGetValue(nj, out value).ToString()+" "+nj.ToString()+" "+value.ToString());
                            if (projet.reseaux[projet.reseau_actif].numnoeud.TryGetValue(nj, out value) == false)
                            {
                                projet.reseaux[projet.reseau_actif].numnoeud.Add(nj, projet.reseaux[projet.reseau_actif].nodes.Count);
                                projet.reseaux[projet.reseau_actif].nodes.Add(nodej);
                            }


                            link lien = new link();
                            lien.no = projet.reseaux[projet.reseau_actif].numnoeud[ni];
                            lien.nd = projet.reseaux[projet.reseau_actif].numnoeud[nj];
                            line = Convert.ToInt32(ch[4]);
                            num_link.i = ni;
                            num_link.j = nj;
                            num_link.line = line;




                            Service num_service = new Service();
                            num_service.numero = -1;
                            if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".")
                            {
                                lien.temps = float.Parse(ch[2].Replace(',', '.'));
                                lien.longueur = float.Parse(ch[3].Replace(',', '.'));
                                num_service.numero = int.Parse(ch[5].Replace(',', '.'));
                                num_service.hd = float.Parse(ch[6].Replace(',', '.'));
                                num_service.hf = float.Parse(ch[7].Replace(',', '.'));
                            }
                            else
                            {
                                lien.temps = float.Parse(ch[2].Replace('.', ','));
                                lien.longueur = float.Parse(ch[3].Replace('.', ','));
                                num_service.numero = int.Parse(ch[5].Replace('.', ','));
                                num_service.hd = float.Parse(ch[6].Replace('.', ','));
                                num_service.hf = float.Parse(ch[7].Replace('.', ','));
                            }
                            if (num_service.hd < 100f && num_service.numero >= 0)
                            {
                                // num_service.hd += 1440f;
                            }
                            if (num_service.hf < 100f && num_service.numero >= 0)
                            {
                                //num_service.hf += 1440f;
                            }
                            if (num_service.hf < num_service.hd)
                            {
                                num_service.hf += 1440f;
                            }

                            if (projet.reseaux[projet.reseau_actif].num_calendrier.TryGetValue(ch[8].ToString().Trim(), out value) == false)
                            {
                                projet.reseaux[projet.reseau_actif].num_calendrier.Add(ch[8].ToString().Trim(), projet.reseaux[projet.reseau_actif].nom_calendrier.Count);
                                projet.reseaux[projet.reseau_actif].nom_calendrier.Add(ch[8].ToString().Trim());

                            }

                            num_service.regime = projet.reseaux[projet.reseau_actif].num_calendrier[ch[8].ToString().Trim()];

                            int nb = projet.reseaux[projet.reseau_actif].links.Count;




                            /*dictionnaire lien*/
                            if (link_id.ContainsKey(num_link) == true && num_service.numero > 0)
                            {
                                projet.reseaux[projet.reseau_actif].links[link_id[num_link]].services.Add(num_service);
                                projet.reseaux[projet.reseau_actif].nbservices += 1;

                            }
                            else
                            {
                                lien.ligne = line;
                                if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".")
                                {
                                    lien.temps = float.Parse(ch[2].Replace(',', '.'));
                                    lien.longueur = float.Parse(ch[3].Replace(',', '.'));
                                }
                                else
                                {
                                    lien.temps = float.Parse(ch[2].Replace('.', ','));
                                    lien.longueur = float.Parse(ch[3].Replace('.', ','));
                                }

                                if (num_service.numero > 0)
                                {
                                    lien.services.Add(num_service);
                                    projet.reseaux[projet.reseau_actif].nbservices += 1;
                                }
                                if (ch.Length > 9)
                                {
                                    if (ch[9].Length > 0)
                                    {
                                        lien.texte = ch[9];
                                    }
                                    else
                                    {
                                        lien.texte = " ";
                                    }
                                }
                                if (ch.Length > 10)
                                {

                                    lien.type = ch[10].Trim().ToString();
                                    if (types.Contains(lien.type) == false)
                                    {
                                        types.Add(lien.type);
                                    }

                                }
                                else
                                {

                                    lien.type = "0";
                                }

                                if (ch.Length > 11)
                                {
                                    if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".")
                                    {
                                        lien.toll = float.Parse(ch[11].Replace(',', '.'));
                                    }
                                    else
                                    {
                                        lien.toll = float.Parse(ch[11].Replace('.', ','));
                                    }
                                }



                                projet.reseaux[projet.reseau_actif].links.Add(lien);
                                link_id[num_link] = projet.reseaux[projet.reseau_actif].links.Count - 1;

                            }

                            /*                        if (nb > 0 )                        
                                                    {
                                                        if (projet.reseaux[projet.reseau_actif].links[nb - 1].no == projet.reseaux[projet.reseau_actif].numnoeud[ni] && projet.reseaux[projet.reseau_actif].links[nb - 1].nd == projet.reseaux[projet.reseau_actif].numnoeud[nj] && projet.reseaux[projet.reseau_actif].links[nb - 1].ligne == line && num_service.numero > 0)
                                                        {

                                                            projet.reseaux[projet.reseau_actif].links[nb - 1].services.Add(num_service);
                                                            projet.reseaux[projet.reseau_actif].nbservices += 1;

                                                        }
                                                        else
                                                        {
                                                            lien.ligne = line;
                                                            if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".")
                                                            {
                                                                lien.temps = float.Parse(ch[2].Replace(',', '.'));
                                                                lien.longueur = float.Parse(ch[3].Replace(',', '.'));
                                                            }
                                                            else
                                                            {
                                                                lien.temps = float.Parse(ch[2].Replace('.', ','));
                                                                lien.longueur = float.Parse(ch[3].Replace('.', ','));
                                                            }

                                                            if (num_service.numero >0)
                                                            {
                                                                lien.services.Add(num_service);
                                                                projet.reseaux[projet.reseau_actif].nbservices += 1;
                                                            }
                                                            if (ch.Length > 9)
                                                            {
                                                                lien.texte = ch[9];
                                                            }
                                                            if (ch.Length > 10)
                                                            {
                                                                lien.type = int.Parse(ch[10]);
                                                                if (lien.type > projet.reseaux[projet.reseau_actif].max_type)
                                                                {
                                                                    projet.reseaux[projet.reseau_actif].max_type = lien.type;

                                                                }
                                                            }

                                                            if (ch.Length > 11)
                                                            {
                                                                if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".")
                                                                {
                                                                    lien.toll = float.Parse(ch[11].Replace(',', '.'));
                                                                }
                                                                else
                                                                {
                                                                    lien.toll = float.Parse(ch[11].Replace('.', ','));
                                                                }
                                                            }



                                                            projet.reseaux[projet.reseau_actif].links.Add(lien);
                                                        }

                                                    }
                                                        else
                                                    {
                                                            lien.ligne = line;
                                                            if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".")
                                                            {
                                                                lien.temps = float.Parse(ch[2].Replace(',','.'));
                                                                lien.longueur = float.Parse(ch[3].Replace(',', '.'));
                                                            }
                                                            else
                                                            {
                                                                lien.temps = float.Parse(ch[2].Replace('.', ','));
                                                                lien.longueur = float.Parse(ch[3].Replace('.', ','));


                                                            }
                                                            if (num_service.numero > 0)
                                                            {
                                                                lien.services.Add(num_service);
                                                                projet.reseaux[projet.reseau_actif].nbservices += 1;
                                                            }
                                                            if (ch.Length > 9)
                                                            {
                                                                lien.texte = ch[9];
                                                            }

                                                            if (ch.Length > 10)
                                                            {
                                                                lien.type = int.Parse(ch[10]);
                                                                if (lien.type > projet.reseaux[projet.reseau_actif].max_type)
                                                                {
                                                                    projet.reseaux[projet.reseau_actif].max_type = lien.type;

                                                                }
                                                            }
                                                            if (ch.Length > 11)
                                                            {
                                                                if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".")
                                                                {
                                                                    lien.toll = float.Parse(ch[11].Replace(',', '.'));
                                                                }
                                                                else
                                                                {
                                                                    lien.toll = float.Parse(ch[11].Replace('.', ','));
                                                                }
                                                            }



                                                            projet.reseaux[projet.reseau_actif].links.Add(lien);
                                                       }


                              */

                        }
                    }
                    fichier_reseau.Close();
                    flux_reseau.Close();

                    /*    for (int k = 0; k <= projet.reseaux[projet.reseau_actif].max_type; k++)
                        {
                            projet.param_affectation_horaire.cveh.Add(1f);
                            projet.param_affectation_horaire.coef_tmap.Add(1f);
                            projet.param_affectation_horaire.cmap.Add(1f);
                            projet.param_affectation_horaire.cboa.Add(1f);
                            projet.param_affectation_horaire.tboa.Add(1f);
                            projet.param_affectation_horaire.cwait.Add(1f);
                            projet.param_affectation_horaire.tboa_max.Add(1f);
                            projet.param_affectation_horaire.ctoll.Add(1f);

                        }*/

                    fich_log.WriteLine("Network:" + nom_reseau);
                    fich_log.WriteLine("Nodes:" + projet.reseaux[projet.reseau_actif].nodes.Count);
                    fich_log.WriteLine("Links:" + projet.reseaux[projet.reseau_actif].links.Count);

                    //construction du graphe
                    // table des prédécesseurs et successeurs de noeuds
                    avancement = 0;
                    Console.WriteLine();
                    ctop = Console.CursorTop;
                    cleft = Console.CursorLeft;

                    for (i = 0; i < projet.reseaux[projet.reseau_actif].links.Count; i++)
                    {
                        //virage.distance = 0;
                        //virage.cout = 0;

                        projet.reseaux[projet.reseau_actif].nodes[projet.reseaux[projet.reseau_actif].links[i].nd].pred.Add(i);
                        projet.reseaux[projet.reseau_actif].nodes[projet.reseaux[projet.reseau_actif].links[i].no].succ.Add(i);
                        //                    Console.SetCursorPosition(1, Console.CursorTop-1);

                        if (avancement < (int)((100 * (i + 1)) / projet.reseaux[projet.reseau_actif].links.Count) - 4)
                        {
                            avancement = (int)((100 * (i + 1)) / projet.reseaux[projet.reseau_actif].links.Count);
                            Console.SetCursorPosition(cleft, ctop);
                            Console.Write("Network topology generation:" + ((100 * (i + 1)) / projet.reseaux[projet.reseau_actif].links.Count).ToString() + "%");

                        }

                    }

                    avancement = 0;

                    // table des prédécesseurs et successeurs de tronçons
                    //Console.WriteLine("création de la topologie des noeuds terminée");

                    Console.WriteLine();
                    ctop = Console.CursorTop;
                    cleft = Console.CursorLeft;

                    /*for (j = 0; j < projet.reseaux[projet.reseau_actif].nodes[projet.reseaux[projet.reseau_actif].links[i].no].pred.Count; j++)
                    {
                        turn virage = new turn();
                        int predecesseur = projet.reseaux[projet.reseau_actif].nodes[projet.reseaux[projet.reseau_actif].links[i].no].pred[j];

                        {
                            virage.numero = predecesseur;
                            virage.temps = 0;
                            projet.reseaux[projet.reseau_actif].links[i].arci.Add(virage);
                            if (projet.reseaux[projet.reseau_actif].links[i].nd == projet.reseaux[projet.reseau_actif].links[predecesseur].no && projet.param_affectation_horaire.demitours == true)
                            {
                                projet.reseaux[projet.reseau_actif].links[i].arci[j].temps = -1;
                                projet.reseaux[projet.reseau_actif].links[i].arci[j].is_valid = true;
                            }
                        }

                    
                    }*/
                    /* for (j = 0; j < projet.reseaux[projet.reseau_actif].nodes[projet.reseaux[projet.reseau_actif].links[i].nd].succ.Count; j++)
                     {
                         turn virage = new turn();
                         int successeur = projet.reseaux[projet.reseau_actif].nodes[projet.reseaux[projet.reseau_actif].links[i].nd].succ[j];
                         {
                             virage.numero = successeur;
                             virage.temps = 0;
                             projet.reseaux[projet.reseau_actif].links[i].arcj.Add(virage);
                             projet.reseaux[projet.reseau_actif].nbturns += 1;
                             if (projet.reseaux[projet.reseau_actif].links[i].no == projet.reseaux[projet.reseau_actif].links[successeur].nd && projet.param_affectation_horaire.demitours == true)
                             {
                                 projet.reseaux[projet.reseau_actif].links[i].arcj[j].temps = -1;
                                 projet.reseaux[projet.reseau_actif].links[i].arcj[j].is_valid = true;

                             }
                         }

                     }*/



                    //fich_log.WriteLine("Virages et correspondances:" + projet.reseaux[projet.reseau_actif].nbturns);
                    fich_log.WriteLine("Time based services:" + projet.reseaux[projet.reseau_actif].nbservices);

                    /*************************Import des pénalités et temps de correspondances************************/
                    /*************************Import des pénalités et temps de correspondances************************/
                    /*************************Import des pénalités et temps de correspondances************************/
                    /*************************Import des pénalités et temps de correspondances************************/
                    /*************************Import des pénalités et temps de correspondances************************/
                    /*************************Import des pénalités et temps de correspondances************************/
                    /*************************Import des pénalités et temps de correspondances************************/

                    if (System.IO.File.Exists(nom_penalites) == true && System.IO.File.Exists(nom_reseau) == true && System.IO.File.Exists(nom_matrice) == true && nom_reseau != null && nom_matrice != null)
                    {
                        fich_log.WriteLine("Penalties and transfers:" + nom_penalites);
                        string[] penal;
                        int ni, nj, nk;
                        int linei, linej, ntri, ntrj;
                        float tps_mvt;
                        System.IO.FileStream flux_penalites;
                        flux_penalites = new System.IO.FileStream(nom_penalites, System.IO.FileMode.Open, FileAccess.Read, FileShare.Read);
                        System.IO.StreamReader fichier_penalites = new System.IO.StreamReader(flux_penalites, System.Text.Encoding.UTF8);
                        while (fichier_penalites.EndOfStream == false)
                        {
                            if (avancement < (int)((100 * flux_penalites.Position) / flux_penalites.Length) - 4)
                            {
                                Console.SetCursorPosition(cleft, ctop);
                                Console.Write("Penalties and transfers import:" + ((100 * flux_penalites.Position) / flux_penalites.Length).ToString() + "%");
                                avancement = (int)((100 * flux_penalites.Position) / flux_penalites.Length);

                            }


                            chaine = fichier_penalites.ReadLine();
                            ntri = -1; ntrj = -1;
                            penal = chaine.Split(param, System.StringSplitOptions.RemoveEmptyEntries);
                            if (penal.Length > 0 && penal[0].Trim().ToUpper() == "P") { LitLigneSousChemin(penal, projet, pats, turns, turns_toll, turns_cost, fich_log); continue; }
                            nj = projet.reseaux[projet.reseau_actif].numnoeud[penal[0].Trim()];
                            ni = projet.reseaux[projet.reseau_actif].numnoeud[penal[1].Trim()];
                            linei = int.Parse(penal[2]);
                            nk = projet.reseaux[projet.reseau_actif].numnoeud[penal[3].Trim()];
                            linej = int.Parse(penal[4]);
                            if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".")
                            {
                                tps_mvt = float.Parse(penal[5].Replace(',', '.'));
                            }
                            else
                            {
                                tps_mvt = float.Parse(penal[5].Replace('.', ','));
                            }
                            float peage_mvt = 0f;
                            if (penal.Length > 6)
                            {
                                if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".") peage_mvt = float.Parse(penal[6].Replace(',', '.'));
                                else peage_mvt = float.Parse(penal[6].Replace('.', ','));
                            }
                            for (i = 0; i < projet.reseaux[projet.reseau_actif].nodes[nj].pred.Count; i++)
                            {
                                if (projet.reseaux[projet.reseau_actif].links[projet.reseaux[projet.reseau_actif].nodes[nj].pred[i]].no == ni && projet.reseaux[projet.reseau_actif].links[projet.reseaux[projet.reseau_actif].nodes[nj].pred[i]].ligne == linei)
                                {
                                    ntri = projet.reseaux[projet.reseau_actif].nodes[nj].pred[i];
                                }
                            }
                            for (i = 0; i < projet.reseaux[projet.reseau_actif].nodes[nj].succ.Count; i++)
                            {
                                if (projet.reseaux[projet.reseau_actif].links[projet.reseaux[projet.reseau_actif].nodes[nj].succ[i]].nd == nk && projet.reseaux[projet.reseau_actif].links[projet.reseaux[projet.reseau_actif].nodes[nj].succ[i]].ligne == linej)
                                {
                                    ntrj = projet.reseaux[projet.reseau_actif].nodes[nj].succ[i];
                                }
                            }
                            if (ntrj >= 0 && ntri >= 0)
                            {
                                Turn virage = new Turn();
                                virage.arci = ntri;
                                virage.arcj = ntrj;
                                float value;
                                if (turns.TryGetValue(virage, out value) == false)
                                {
                                    turns.Add(virage, tps_mvt);
                                    if (peage_mvt != 0f) turns_toll.Add(virage, peage_mvt);
                                }
                                projet.reseaux[projet.reseau_actif].nodes[nj].is_intersection = true;
                                /*  for (i = 0; i < projet.reseaux[projet.reseau_actif].links[ntrj].arci.Count; i++)
                                  {
                                      if (projet.reseaux[projet.reseau_actif].links[ntrj].arci[i].numero == ntri)
                                      {
                                          projet.reseaux[projet.reseau_actif].links[ntrj].arci[i].temps = tps_mvt;
                                          projet.reseaux[projet.reseau_actif].links[ntrj].arci[i].is_valid = true;

                                      }
                                  }
                                  for (i = 0; i < projet.reseaux[projet.reseau_actif].links[ntri].arcj.Count; i++)
                                  {
                                      if (projet.reseaux[projet.reseau_actif].links[ntri].arcj[i].numero == ntrj)
                                      {
                                          projet.reseaux[projet.reseau_actif].links[ntri].arcj[i].temps = tps_mvt;
                                          projet.reseaux[projet.reseau_actif].links[ntri].arcj[i].is_valid = true;

                                      }
                                  }*/
                            }

                        }
                        fichier_penalites.Close();
                        flux_penalites.Close();

                    }
                    Console.SetCursorPosition(cleft, ctop);
                    Console.Write("Penalties and transfers import:" + (100).ToString() + "%");


                    ////écrire réseau en XML////

                    /*network Export = projet.reseaux[projet.reseau_actif];
                    System.Xml.Serialization.XmlSerializer writer =
                    new System.Xml.Serialization.XmlSerializer(Export.GetType());
                    System.IO.StreamWriter file = new System.IO.StreamWriter( projet.param_affectation_horaire.nom_sortie+ ".xml");

                    writer.Serialize(file, Export);
                    file.Close();*/



                    //affectation tc à horaires algorithme
                    // graph growth aglorithm with buckets
                    // graph growth aglorithm with buckets
                    // graph growth aglorithm with buckets
                    // graph growth aglorithm with buckets
                    // graph growth aglorithm with buckets
                    // graph growth aglorithm with buckets
                    // graph growth aglorithm with buckets
                    // graph growth aglorithm with buckets
                    if (projet.param_affectation_horaire.algorithme <= 1)
                    {


                        System.IO.StreamWriter fich_sortie = new System.IO.StreamWriter(projet.param_affectation_horaire.nom_sortie + "_temps.txt", false, Encoding.UTF8);
                        System.IO.StreamWriter fich_sortie2 = new System.IO.StreamWriter(projet.param_affectation_horaire.nom_sortie + "_chemins.txt", false, Encoding.UTF8);
                        System.IO.StreamWriter fich_result = new System.IO.StreamWriter(projet.param_affectation_horaire.nom_sortie + "_aff.txt", false, Encoding.UTF8);
                        System.IO.StreamWriter fich_od = new System.IO.StreamWriter(projet.param_affectation_horaire.nom_sortie + "_od.txt", false, Encoding.UTF8);
                        System.IO.StreamWriter fich_noeuds = new System.IO.StreamWriter(projet.param_affectation_horaire.nom_sortie + "_noeuds.txt", false, Encoding.UTF8);
                        System.IO.StreamWriter fich_detour = new System.IO.StreamWriter(projet.param_affectation_horaire.nom_sortie + "_detour.txt", false, Encoding.UTF8);
                        System.IO.StreamWriter fich_isoles = new System.IO.StreamWriter(projet.param_affectation_horaire.nom_sortie + "_isoles.txt", false, Encoding.UTF8);
                        Ecrit_parametres(projet.param_affectation_horaire, projet.param_affectation_horaire.nom_sortie + "_param.txt");
                        fich_sortie2.WriteLine("id;o;d;jour;heure;i;j;ij;ligne;service;temps;heureo;tveh;tmap;tatt;tcorr;ncorr;tatt1;cout;longueur;pole;volau;boai;alij;texte;type;toll");
                        fich_result.WriteLine("i;j;ligne;volau;boai;alij;texte;type;toll");
                        fich_od.WriteLine("id;o;d;jour;heureo;heured;temps;tveh;tmap;tatt;tcorr;ncorr;tatt1;cout;longueur;pole;volau;texte;nbpop;toll");
                        if (projet.param_affectation_horaire.sortie_temps == 3)
                        {
                            fich_sortie.WriteLine("o;ij;ligne;temps;tatt1;volau");
                            fich_noeuds.WriteLine("o;numero;temps;tatt1;volau");

                        }
                        else
                        {
                            fich_sortie.WriteLine("id;o;ij;ligne;numtrc;jour;heureo;heured;temps;tveh;tmap;tatt;tcorr;ncorr;tatt1;cout;longueur;pole;volau;precedent;type;toll;ti");
                            fich_noeuds.WriteLine("id;o;d;jour;numero;heureo;heured;temps;tveh;tmap;tatt;tcorr;ncorr;tatt1;cout;longueur;pole;toll;volau;stops");
                        }



                        // Console.WriteLine("création de la topologie des tronçons terminée");
                        //plus courts chemins
                        Queue<int> touches = new Queue<int>();
                        Queue<int> calcules = new Queue<int>();
                        List<List<int>> gga_nq = new List<List<int>>();

                        avancement = 0;
                        Console.WriteLine();
                        ctop = Console.CursorTop;
                        cleft = Console.CursorLeft;


                        //initilisation
                        for (i = 0; i < projet.reseaux[projet.reseau_actif].links.Count; i++)
                        {
                            projet.reseaux[projet.reseau_actif].links[i].l = 0;
                            projet.reseaux[projet.reseau_actif].links[i].volau = 0;
                            projet.reseaux[projet.reseau_actif].links[i].touche = 0;
                            projet.reseaux[projet.reseau_actif].links[i].cout = 0;
                            projet.reseaux[projet.reseau_actif].links[i].pivot = -1;
                            projet.reseaux[projet.reseau_actif].links[i].is_queue = false;
                            //                projet.reseaux[projet.reseau_actif].links[i].temps = projet.reseaux[projet.reseau_actif].links[i].fd(projet.reseaux[projet.reseau_actif].links[i].volau, projet.reseaux[projet.reseau_actif].links[i].longueur, 0f, projet.reseaux[projet.reseau_actif].links[i].lanes * 1000, projet.reseaux[projet.reseau_actif].links[i].v0, projet.reseaux[projet.reseau_actif].links[i].a, projet.reseaux[projet.reseau_actif].links[i].b, projet.reseaux[projet.reseau_actif].links[i].n);

                        }


                        string p, q, p1 = "", q1 = "", libod = "";
                        int sens = 1, sens1 = 0, jour1 = 0, numod = 0;
                        float horaire1 = 0;

                        System.IO.FileStream flux_matrice;

                        flux_matrice = new System.IO.FileStream(nom_matrice, System.IO.FileMode.Open, FileAccess.Read, System.IO.FileShare.Read);
                        System.IO.StreamReader fichier_matrice = new System.IO.StreamReader(flux_matrice, System.Text.Encoding.UTF8);
                        avancement = 0;
                        fich_log.WriteLine("Matrix:" + nom_matrice);
                        DateTime t1 = DateTime.Now;
                        fich_log.WriteLine("Computation start time: " + t1.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
                        fich_log.Flush();
                        // ponderations avance/retard du fichier de parametres (valeurs par defaut des lignes de matrice sans colonnes 25/26)
                        EtatsData etats = ConstruitEtats(projet, pats, turns, turns_toll, turns_cost);
                        if (etats.nOrig > 0) fich_log.WriteLine("Sub-path penalties: " + pats.Count + " sub-paths of 3 arcs or more, " + etats.nEtats + " arc copies");
                        Param_affectation_horaire prm0 = aff_hor.CloneParam(); // parametres de reference du fichier (apres Ecrit_parametres)
                        ParamSnap snap1 = null;                                  // parametres de la ligne precedente
                        bool av0_actif = aff_hor.avance_retard;
                        float av0_a = aff_hor.pond_avance, av0_r = aff_hor.pond_retard;
                        OdParallel od_parallel = new OdParallel(projet, turns, turns_toll, turns_cost, etats, transfers, flux_matrice.Length, ctop, cleft, avancement,
                            fich_sortie, fich_sortie2, fich_od, fich_noeuds, fich_detour, fich_isoles, fich_log);
                    lec1:
                        while (fichier_matrice.EndOfStream == false)
                        {
                        lecture:
                            projet.param_affectation_horaire.nb_pop = 0;
                            projet.param_affectation_horaire.RestaurerDepuis(prm0); // une ligne n'herite plus des parametres de la precedente
                            chaine = fichier_matrice.ReadLine();
                            long posLecture = flux_matrice.Position; // l'affichage d'avancement est fait, dans l'ordre, par OdParallel
                            if (chaine.Trim().Length == 0) goto lec1;
                            if (chaine == "")
                            {
                                goto lecture;
                            }
                            numod++;
                            ch = chaine.Split(param, StringSplitOptions.RemoveEmptyEntries);
                            int nbCols = ch.Length;
                            p = ch[0].Trim();
                            q = ch[1].Trim();
                            float od, horaire;
                            int jour;
                            if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".")
                            {
                                od = Single.Parse(ch[2].Replace(',', '.'));
                                jour = (int)Single.Parse(ch[3].Replace(',', '.'));
                                horaire = Single.Parse(ch[4].Replace(',', '.'));
                            }
                            else
                            {
                                od = Single.Parse(ch[2].Replace('.', ','));
                                jour = (int)Single.Parse(ch[3].Replace('.', ','));
                                horaire = Single.Parse(ch[4].Replace('.', ','));
                            }
                            if (ch.Length > 5)
                            {
                                if (ch[5].ToLower() == "d")
                                {
                                    sens = 1;

                                }
                                else if (ch[5].ToLower() == "a")
                                {
                                    sens = 2;
                                }
                            }
                            else
                            {
                                sens = 1;
                            }
                            if (ch.Length > 6)
                            {
                                if (ch[6].Length == 0)
                                {
                                    libod = numod.ToString();

                                }
                                else
                                {
                                    libod = ch[6].Trim();
                                }
                                libod = ch[6];
                            }
                            else
                            {
                                libod = numod.ToString();
                            }
                            if (ch.Length > 17)
                            {
                                string[] type_delim = { "|" };
                                int k;
                                string[] scveh, scwait, scmap, scboa, scoef_tmap, stboa, stboa_max, stoll;
                                if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".")
                                {
                                    scveh = ch[7].Replace(",", ".").Split(type_delim, StringSplitOptions.None);
                                    scwait = ch[8].Replace(",", ".").Split(type_delim, StringSplitOptions.None);
                                    scmap = ch[9].Replace(",", ".").Split(type_delim, StringSplitOptions.None);
                                    scboa = ch[10].Replace(",", ".").Split(type_delim, StringSplitOptions.None);
                                    scoef_tmap = ch[11].Replace(",", ".").Split(type_delim, StringSplitOptions.None);
                                    stboa = ch[12].Replace(",", ".").Split(type_delim, StringSplitOptions.None);
                                    stboa_max = ch[13].Replace(",", ".").Split(type_delim, StringSplitOptions.None);
                                    //    stmap_max = ch[13].Replace(",", ".").Split(type_delim, StringSplitOptions.None);
                                    stoll = ch[16].Replace(",", ".").Split(type_delim, StringSplitOptions.None);

                                }
                                else
                                {
                                    scveh = ch[7].Replace(".", ",").Split(type_delim, StringSplitOptions.None);
                                    scwait = ch[8].Replace(".", ",").Split(type_delim, StringSplitOptions.None);
                                    scmap = ch[9].Replace(".", ",").Split(type_delim, StringSplitOptions.None);
                                    scboa = ch[10].Replace(".", ",").Split(type_delim, StringSplitOptions.None);
                                    scoef_tmap = ch[11].Replace(".", ",").Split(type_delim, StringSplitOptions.None);
                                    stboa = ch[12].Replace(".", ",").Split(type_delim, StringSplitOptions.None);
                                    stboa_max = ch[13].Replace(".", ",").Split(type_delim, StringSplitOptions.None);
                                    //  stmap_max = ch[13].Replace(".", ",").Split(type_delim, StringSplitOptions.None);
                                    stoll = ch[16].Replace(".", ",").Split(type_delim, StringSplitOptions.None);
                                }
                                projet.param_affectation_horaire.texte_cveh = ch[7];
                                projet.param_affectation_horaire.texte_cwait = ch[8];
                                projet.param_affectation_horaire.texte_cmap = ch[9];
                                projet.param_affectation_horaire.texte_cboa = ch[10];
                                projet.param_affectation_horaire.texte_coef_tmap = ch[11];
                                projet.param_affectation_horaire.texte_tboa = ch[12];
                                projet.param_affectation_horaire.texte_tboa_max = ch[13];
                                //                        projet.param_affectation_horaire.texte_tboa_max = ch[12];
                                projet.param_affectation_horaire.texte_toll = ch[16];


                                //pondérations temps TC par type
                                for (k = 0; k < scveh.Length; k++)
                                {
                                    string[] keys;
                                    string[] sep = { ":" };
                                    keys = scveh[k].Split(sep, StringSplitOptions.None);
                                    if (keys.Length == 1)
                                    {
                                        projet.param_affectation_horaire.cveh["0"] = float.Parse(keys[0]);
                                    }
                                    else
                                    {
                                        projet.param_affectation_horaire.cveh[keys[0].Trim()] = float.Parse(keys[1]);
                                    }
                                }
                                //pondérations temps attente par type
                                for (k = 0; k < scwait.Length; k++)
                                {
                                    string[] keys;
                                    string[] sep = { ":" };
                                    keys = scwait[k].Split(sep, StringSplitOptions.None);
                                    if (keys.Length == 1)
                                    {
                                        projet.param_affectation_horaire.cwait["0"] = float.Parse(keys[0]);
                                    }
                                    else
                                    {
                                        projet.param_affectation_horaire.cwait[keys[0].Trim()] = float.Parse(keys[1]);
                                    }
                                }
                                //pondérations temps marche par type
                                for (k = 0; k < scmap.Length; k++)
                                {
                                    string[] keys;
                                    string[] sep = { ":" };
                                    keys = scmap[k].Split(sep, StringSplitOptions.None);
                                    if (keys.Length == 1)
                                    {
                                        projet.param_affectation_horaire.cmap["0"] = float.Parse(keys[0]);
                                    }
                                    else
                                    {
                                        projet.param_affectation_horaire.cmap[keys[0].Trim()] = float.Parse(keys[1]);
                                    }
                                }
                                //pondérations correspondance par type
                                for (k = 0; k < scboa.Length; k++)
                                {
                                    string[] keys;
                                    string[] sep = { ":" };
                                    keys = scboa[k].Split(sep, StringSplitOptions.None);
                                    if (keys.Length == 1)
                                    {
                                        projet.param_affectation_horaire.cboa["0"] = float.Parse(keys[0]);
                                    }
                                    else
                                    {
                                        projet.param_affectation_horaire.cboa[keys[0].Trim()] = float.Parse(keys[1]);
                                    }
                                }
                                //pondérations coef vitesse marche par type
                                for (k = 0; k < scoef_tmap.Length; k++)
                                {
                                    string[] keys;
                                    string[] sep = { ":" };
                                    keys = scoef_tmap[k].Split(sep, StringSplitOptions.None);
                                    if (keys.Length == 1)
                                    {
                                        projet.param_affectation_horaire.coef_tmap["0"] = float.Parse(keys[0]);
                                    }
                                    else
                                    {
                                        projet.param_affectation_horaire.coef_tmap[keys[0].Trim()] = float.Parse(keys[1]);
                                    }
                                }
                                //temps correspondance par type
                                for (k = 0; k < stboa.Length; k++)
                                {
                                    string[] keys;
                                    string[] sep = { ":" };
                                    keys = stboa[k].Split(sep, StringSplitOptions.None);
                                    if (keys.Length == 1)
                                    {
                                        projet.param_affectation_horaire.tboa["0"] = float.Parse(keys[0]);
                                    }
                                    else
                                    {
                                        projet.param_affectation_horaire.tboa[keys[0].Trim()] = float.Parse(keys[1]);
                                    }
                                }
                                //temps correspondance maximum par type
                                for (k = 0; k < stboa_max.Length; k++)
                                {
                                    string[] keys;
                                    string[] sep = { ":" };
                                    keys = stboa_max[k].Split(sep, StringSplitOptions.None);
                                    if (keys.Length == 1)
                                    {
                                        projet.param_affectation_horaire.tboa_max["0"] = float.Parse(keys[0]);
                                    }
                                    else
                                    {
                                        projet.param_affectation_horaire.tboa_max[keys[0].Trim()] = float.Parse(keys[1]);
                                    }
                                }
                                //pondération péage par type
                                for (k = 0; k < stoll.Length; k++)
                                {
                                    string[] keys;
                                    string[] sep = { ":" };
                                    keys = stoll[k].Split(sep, StringSplitOptions.None);
                                    if (keys.Length == 1)
                                    {
                                        projet.param_affectation_horaire.ctoll["0"] = float.Parse(keys[0]);
                                    }
                                    else
                                    {
                                        projet.param_affectation_horaire.ctoll[keys[0].Trim()] = float.Parse(keys[1]);
                                    }
                                }

                                foreach (String cle in types)
                                {
                                    if (projet.param_affectation_horaire.cveh.ContainsKey(cle) == false)
                                    {
                                        projet.param_affectation_horaire.cveh[cle] = projet.param_affectation_horaire.cveh["0"];
                                    }
                                    if (projet.param_affectation_horaire.cmap.ContainsKey(cle) == false)
                                    {
                                        projet.param_affectation_horaire.cmap[cle] = projet.param_affectation_horaire.cmap["0"];
                                    }
                                    if (projet.param_affectation_horaire.cwait.ContainsKey(cle) == false)
                                    {
                                        projet.param_affectation_horaire.cwait[cle] = projet.param_affectation_horaire.cwait["0"];
                                    }
                                    if (projet.param_affectation_horaire.cboa.ContainsKey(cle) == false)
                                    {
                                        projet.param_affectation_horaire.cboa[cle] = projet.param_affectation_horaire.cboa["0"];
                                    }
                                    if (projet.param_affectation_horaire.tboa.ContainsKey(cle) == false)
                                    {
                                        projet.param_affectation_horaire.tboa[cle] = projet.param_affectation_horaire.tboa["0"];
                                    }
                                    if (projet.param_affectation_horaire.coef_tmap.ContainsKey(cle) == false)
                                    {
                                        projet.param_affectation_horaire.coef_tmap[cle] = projet.param_affectation_horaire.coef_tmap["0"];
                                    }
                                    if (projet.param_affectation_horaire.tboa_max.ContainsKey(cle) == false)
                                    {
                                        projet.param_affectation_horaire.tboa_max[cle] = projet.param_affectation_horaire.tboa_max["0"];
                                    }
                                    if (projet.param_affectation_horaire.ctoll.ContainsKey(cle) == false)
                                    {
                                        projet.param_affectation_horaire.ctoll[cle] = projet.param_affectation_horaire.ctoll["0"];
                                    }
                                }
                                if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".")
                                {
                                    projet.param_affectation_horaire.nb_jours = int.Parse(ch[14].Split(type_delim, StringSplitOptions.None)[0]);
                                    projet.param_affectation_horaire.tmapmax = float.Parse(ch[15].Replace(',', '.').Split(type_delim, StringSplitOptions.None)[0]);
                                    projet.param_affectation_horaire.temps_max = float.Parse(ch[17].Replace(',', '.').Split(type_delim, StringSplitOptions.None)[0]);

                                }
                                else
                                {
                                    projet.param_affectation_horaire.nb_jours = int.Parse(ch[14].Split(type_delim, StringSplitOptions.None)[0]);
                                    projet.param_affectation_horaire.tmapmax = float.Parse(ch[15].Replace('.', ',').Split(type_delim, StringSplitOptions.None)[0]);
                                    projet.param_affectation_horaire.temps_max = float.Parse(ch[17].Replace('.', ',').Split(type_delim, StringSplitOptions.None)[0]);

                                }

                                /*                        for (k = 0; k <= projet.reseaux[projet.reseau_actif].max_type; k++)
                                                        {



                                                            if (k < scveh.Length)
                                                            {
                                                                projet.param_affectation_horaire.cveh[k]= float.Parse(scveh[k]);
                                                            }
                                                            else
                                                            {
                                                                projet.param_affectation_horaire.cveh[k] = float.Parse(scveh[0]);
                                                            }
                                                            if (k < scwait.Length)
                                                            {
                                                                projet.param_affectation_horaire.cwait[k]= float.Parse(scwait[k]);
                                                            }
                                                            else
                                                            {
                                                                projet.param_affectation_horaire.cwait[k] = float.Parse(scwait[0]);
                                                            }
                                                            if (k < scmap.Length)
                                                            {
                                                                projet.param_affectation_horaire.cmap[k]= float.Parse(scmap[k]);
                                                            }
                                                            else
                                                            {
                                                                projet.param_affectation_horaire.cmap[k] = float.Parse(scmap[0]);
                                                            }
                                                            if (k < scboa.Length)
                                                            {
                                                                projet.param_affectation_horaire.cboa[k]= float.Parse(scboa[k]);
                                                            }
                                                            else
                                                            {
                                                                projet.param_affectation_horaire.cboa[k] = float.Parse(scboa[0]);
                                                            }
                                                            if (k < scoef_tmap.Length)
                                                            {
                                                                projet.param_affectation_horaire.coef_tmap[k]= float.Parse(scoef_tmap[k]);
                                                            }
                                                            else
                                                            {
                                                                projet.param_affectation_horaire.coef_tmap[k] = float.Parse(scoef_tmap[0]);
                                                            }
                                                            if (k < stboa.Length)
                                                            {
                                                                projet.param_affectation_horaire.tboa[k]= float.Parse(stboa[k]);
                                                            }
                                                            else
                                                            {
                                                                projet.param_affectation_horaire.tboa[k] = float.Parse(stboa[0]);
                                                            }
                                                            if (k < stboa_max.Length)
                                                            {
                                                                projet.param_affectation_horaire.tboa_max[k] = float.Parse(stboa_max[k]);
                                                            }
                                                            else
                                                            {
                                                                projet.param_affectation_horaire.tboa_max[k] = float.Parse(stboa_max[0]);
                                                            }
                                                            if (k < stoll.Length)
                                                            {
                                                                projet.param_affectation_horaire.ctoll[k] = float.Parse(stoll[k]);
                                                            }
                                                            else
                                                            {
                                                                projet.param_affectation_horaire.ctoll[k] = float.Parse(stoll[0]);
                                                            }
                                                            projet.param_affectation_horaire.nb_jours = int.Parse(ch[13].Split(type_delim, StringSplitOptions.None)[0]);
                                                            projet.param_affectation_horaire.tmapmax = int.Parse(ch[14].Split(type_delim, StringSplitOptions.None)[0]);


                                                        }*/

                            }
                            else
                            {
                                string[] type_delim = { "|" };
                                int k;
                                string[] scveh, scwait, scmap, scboa, scoef_tmap, stboa, stboa_max, stoll;
                                if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".")
                                {
                                    scveh = aff_hor.texte_cveh.Replace(",", ".").Split(type_delim, StringSplitOptions.None);
                                    scwait = aff_hor.texte_cwait.Replace(",", ".").Split(type_delim, StringSplitOptions.None);
                                    scmap = aff_hor.texte_cmap.Replace(",", ".").Split(type_delim, StringSplitOptions.None);
                                    scboa = aff_hor.texte_cboa.Replace(",", ".").Split(type_delim, StringSplitOptions.None);
                                    scoef_tmap = aff_hor.texte_coef_tmap.Replace(",", ".").Split(type_delim, StringSplitOptions.None);
                                    stboa = aff_hor.texte_tboa.Replace(",", ".").Split(type_delim, StringSplitOptions.None);
                                    stboa_max = aff_hor.texte_tboa_max.Replace(",", ".").Split(type_delim, StringSplitOptions.None);
                                    stoll = aff_hor.texte_toll.Replace(",", ".").Split(type_delim, StringSplitOptions.None);
                                }
                                else
                                {
                                    scveh = aff_hor.texte_cveh.Replace(".", ",").Split(type_delim, StringSplitOptions.None);
                                    scwait = aff_hor.texte_cwait.Replace(".", ",").Split(type_delim, StringSplitOptions.None);
                                    scmap = aff_hor.texte_cmap.Replace(".", ",").Split(type_delim, StringSplitOptions.None);
                                    scboa = aff_hor.texte_cboa.Replace(".", ",").Split(type_delim, StringSplitOptions.None);
                                    scoef_tmap = aff_hor.texte_coef_tmap.Replace(".", ",").Split(type_delim, StringSplitOptions.None);
                                    stboa = aff_hor.texte_tboa.Replace(".", ",").Split(type_delim, StringSplitOptions.None);
                                    stboa_max = aff_hor.texte_tboa_max.Replace(".", ",").Split(type_delim, StringSplitOptions.None);
                                    stoll = aff_hor.texte_toll.Replace(",", ".").Split(type_delim, StringSplitOptions.None);
                                }



                                projet.param_affectation_horaire = aff_hor;
                                //pondérations temps TC par type
                                for (k = 0; k < scveh.Length; k++)
                                {
                                    string[] keys;
                                    string[] sep = { ":" };
                                    keys = scveh[k].Split(sep, StringSplitOptions.None);
                                    if (keys.Length == 1)
                                    {
                                        projet.param_affectation_horaire.cveh["0"] = float.Parse(keys[0]);
                                    }
                                    else
                                    {
                                        projet.param_affectation_horaire.cveh[keys[0].Trim()] = float.Parse(keys[1]);
                                    }
                                }
                                //pondérations temps attente par type
                                for (k = 0; k < scwait.Length; k++)
                                {
                                    string[] keys;
                                    string[] sep = { ":" };
                                    keys = scwait[k].Split(sep, StringSplitOptions.None);
                                    if (keys.Length == 1)
                                    {
                                        projet.param_affectation_horaire.cwait["0"] = float.Parse(keys[0]);
                                    }
                                    else
                                    {
                                        projet.param_affectation_horaire.cwait[keys[0].Trim()] = float.Parse(keys[1]);
                                    }
                                }
                                //pondérations temps marche par type
                                for (k = 0; k < scmap.Length; k++)
                                {
                                    string[] keys;
                                    string[] sep = { ":" };
                                    keys = scmap[k].Split(sep, StringSplitOptions.None);
                                    if (keys.Length == 1)
                                    {
                                        projet.param_affectation_horaire.cmap["0"] = float.Parse(keys[0]);
                                    }
                                    else
                                    {
                                        projet.param_affectation_horaire.cmap[keys[0].Trim()] = float.Parse(keys[1]);
                                    }
                                }
                                //pondérations correspondance par type
                                for (k = 0; k < scboa.Length; k++)
                                {
                                    string[] keys;
                                    string[] sep = { ":" };
                                    keys = scboa[k].Split(sep, StringSplitOptions.None);
                                    if (keys.Length == 1)
                                    {
                                        projet.param_affectation_horaire.cboa["0"] = float.Parse(keys[0]);
                                    }
                                    else
                                    {
                                        projet.param_affectation_horaire.cboa[keys[0].Trim()] = float.Parse(keys[1]);
                                    }
                                }
                                //pondérations coef vitesse marche par type
                                for (k = 0; k < scoef_tmap.Length; k++)
                                {
                                    string[] keys;
                                    string[] sep = { ":" };
                                    keys = scoef_tmap[k].Split(sep, StringSplitOptions.None);
                                    if (keys.Length == 1)
                                    {
                                        projet.param_affectation_horaire.coef_tmap["0"] = float.Parse(keys[0]);
                                    }
                                    else
                                    {
                                        projet.param_affectation_horaire.coef_tmap[keys[0].Trim()] = float.Parse(keys[1]);
                                    }
                                }
                                //temps correspondance par type
                                for (k = 0; k < stboa.Length; k++)
                                {
                                    string[] keys;
                                    string[] sep = { ":" };
                                    keys = stboa[k].Split(sep, StringSplitOptions.None);
                                    if (keys.Length == 1)
                                    {
                                        projet.param_affectation_horaire.tboa["0"] = float.Parse(keys[0]);
                                    }
                                    else
                                    {
                                        projet.param_affectation_horaire.tboa[keys[0].Trim()] = float.Parse(keys[1]);
                                    }
                                }
                                //temps correspondance maximum par type
                                for (k = 0; k < stboa_max.Length; k++)
                                {
                                    string[] keys;
                                    string[] sep = { ":" };
                                    keys = stboa_max[k].Split(sep, StringSplitOptions.None);
                                    if (keys.Length == 1)
                                    {
                                        projet.param_affectation_horaire.tboa_max["0"] = float.Parse(keys[0]);
                                    }
                                    else
                                    {
                                        projet.param_affectation_horaire.tboa_max[keys[0].Trim()] = float.Parse(keys[1]);
                                    }
                                }
                                //pondération péage par type
                                for (k = 0; k < stoll.Length; k++)
                                {
                                    string[] keys;
                                    string[] sep = { ":" };
                                    keys = stoll[k].Split(sep, StringSplitOptions.None);
                                    if (keys.Length == 1)
                                    {
                                        projet.param_affectation_horaire.ctoll["0"] = float.Parse(keys[0]);
                                    }
                                    else
                                    {
                                        projet.param_affectation_horaire.ctoll[keys[0].Trim()] = float.Parse(keys[1]);
                                    }
                                }

                                foreach (String cle in types)
                                {
                                    if (projet.param_affectation_horaire.cveh.ContainsKey(cle) == false)
                                    {
                                        projet.param_affectation_horaire.cveh[cle] = projet.param_affectation_horaire.cveh["0"];
                                    }
                                    if (projet.param_affectation_horaire.cmap.ContainsKey(cle) == false)
                                    {
                                        projet.param_affectation_horaire.cmap[cle] = projet.param_affectation_horaire.cmap["0"];
                                    }
                                    if (projet.param_affectation_horaire.cwait.ContainsKey(cle) == false)
                                    {
                                        projet.param_affectation_horaire.cwait[cle] = projet.param_affectation_horaire.cwait["0"];
                                    }
                                    if (projet.param_affectation_horaire.cboa.ContainsKey(cle) == false)
                                    {
                                        projet.param_affectation_horaire.cboa[cle] = projet.param_affectation_horaire.cboa["0"];
                                    }
                                    if (projet.param_affectation_horaire.tboa.ContainsKey(cle) == false)
                                    {
                                        projet.param_affectation_horaire.tboa[cle] = projet.param_affectation_horaire.tboa["0"];
                                    }
                                    if (projet.param_affectation_horaire.coef_tmap.ContainsKey(cle) == false)
                                    {
                                        projet.param_affectation_horaire.coef_tmap[cle] = projet.param_affectation_horaire.coef_tmap["0"];
                                    }
                                    if (projet.param_affectation_horaire.tboa_max.ContainsKey(cle) == false)
                                    {
                                        projet.param_affectation_horaire.tboa_max[cle] = projet.param_affectation_horaire.tboa_max["0"];
                                    }
                                    if (projet.param_affectation_horaire.ctoll.ContainsKey(cle) == false)
                                    {
                                        projet.param_affectation_horaire.ctoll[cle] = projet.param_affectation_horaire.ctoll["0"];
                                    }
                                }


                                /*                            for (k = 0; k <= projet.reseaux[projet.reseau_actif].max_type; k++)
                                                            {


                                                                if (k < scveh.Length)
                                                                {
                                                                    projet.param_affectation_horaire.cveh[k]= float.Parse(scveh[k]);
                                                                }
                                                                else
                                                                {
                                                                    projet.param_affectation_horaire.cveh[k]=float.Parse(scveh[0]);
                                                                }
                                                                if (k <scwait.Length)
                                                                {
                                                                    projet.param_affectation_horaire.cwait[k]= float.Parse(scwait[k]);
                                                                }
                                                                else
                                                                {
                                                                    projet.param_affectation_horaire.cwait[k]= float.Parse(scwait[0]);
                                                                }
                                                                if (k < scmap.Length)
                                                                {
                                                                    projet.param_affectation_horaire.cmap[k]= float.Parse(scmap[k]);
                                                                }
                                                                else
                                                                {
                                                                    projet.param_affectation_horaire.cmap[k]= float.Parse(scmap[0]);
                                                                }
                                                                if (k < scboa.Length)
                                                                {
                                                                    projet.param_affectation_horaire.cboa[k]= float.Parse(scboa[k]);
                                                                }
                                                                else
                                                                {
                                                                    projet.param_affectation_horaire.cboa[k]= float.Parse(scboa[0]);
                                                                }
                                                                if (k < scoef_tmap.Length)
                                                                {
                                                                    projet.param_affectation_horaire.coef_tmap[k]= float.Parse(scoef_tmap[k]);
                                                                }
                                                                else
                                                                {
                                                                    projet.param_affectation_horaire.coef_tmap[k]= float.Parse(scoef_tmap[0]);
                                                                }
                                                                if (k < stboa.Length)
                                                                {
                                                                    projet.param_affectation_horaire.tboa[k]= float.Parse(stboa[k]);
                                                                }
                                                                else
                                                                {
                                                                    projet.param_affectation_horaire.tboa[k]=float.Parse(stboa[0]);                                
                                                                }
                                                                if (k < stboa_max.Length)
                                                                {
                                                                    projet.param_affectation_horaire.tboa_max[k] = float.Parse(stboa_max[k]);
                                                                }
                                                                else
                                                                {
                                                                    projet.param_affectation_horaire.tboa_max[k] = float.Parse(stboa_max[0]);
                                                                }
                                                                if (k < stoll.Length)
                                                                {
                                                                    projet.param_affectation_horaire.ctoll[k] = float.Parse(stoll[k]);
                                                                }
                                                                else
                                                                {
                                                                    projet.param_affectation_horaire.ctoll[k] = float.Parse(stoll[0]);
                                                                }

                                                            }

                                                            */


                            }
                            if (ch.Length > 23)
                            {
                                if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".")
                                {
                                    projet.param_affectation_horaire.sortie_chemins = bool.Parse(ch[18].Replace(",", "."));
                                    projet.param_affectation_horaire.sortie_temps = int.Parse(ch[19].Replace(",", "."));
                                    projet.param_affectation_horaire.algorithme = int.Parse(ch[20].Replace(",", "."));
                                    projet.param_affectation_horaire.param_dijkstra = float.Parse(ch[21].Replace(",", "."));
                                    projet.param_affectation_horaire.max_nb_buckets = float.Parse(ch[22].Replace(",", "."));
                                    projet.param_affectation_horaire.pu = float.Parse(ch[23].Replace(",", "."));
                                }
                                else
                                {
                                    projet.param_affectation_horaire.sortie_chemins = bool.Parse(ch[18].Replace(".", ","));
                                    projet.param_affectation_horaire.sortie_temps = int.Parse(ch[19].Replace(".", ","));
                                    projet.param_affectation_horaire.algorithme = int.Parse(ch[20].Replace(".", ","));
                                    projet.param_affectation_horaire.param_dijkstra = float.Parse(ch[21].Replace(".", ","));
                                    projet.param_affectation_horaire.max_nb_buckets = float.Parse(ch[22].Replace(".", ","));
                                    projet.param_affectation_horaire.pu = float.Parse(ch[23].Replace(".", ","));

                                }

                            }
                            else
                            {
                                projet.param_affectation_horaire.sortie_chemins = aff_hor.sortie_chemins;
                                projet.param_affectation_horaire.sortie_temps = aff_hor.sortie_temps;
                                if (aff_hor.sortie_temps >= 10)
                                {
                                    aff_hor.sortie_stops = true;
                                    aff_hor.sortie_temps += -10;
                                }
                                else
                                {
                                    aff_hor.sortie_stops = false;
                                }
                                projet.param_affectation_horaire.algorithme = aff_hor.algorithme;
                                projet.param_affectation_horaire.param_dijkstra = aff_hor.param_dijkstra;
                                projet.param_affectation_horaire.max_nb_buckets = aff_hor.max_nb_buckets;
                                projet.param_affectation_horaire.pu = aff_hor.pu;

                            }


                            if (ch.Length > 24)
                            {
                                projet.param_affectation_horaire.texte_filtre_sortie = ch[24];
                            }
                            // ponderations avance/retard propres a la ligne (colonnes 25 et 26) ; a defaut, valeurs du fichier de parametres
                            projet.param_affectation_horaire.avance_retard = av0_actif;
                            projet.param_affectation_horaire.pond_avance = av0_a;
                            projet.param_affectation_horaire.pond_retard = av0_r;
                            if (ch.Length > 25)
                            {
                                projet.param_affectation_horaire.pond_avance = LitFlottantCulture(ch[25].Trim());
                                projet.param_affectation_horaire.avance_retard = true;
                            }
                            if (ch.Length > 26)
                            {
                                projet.param_affectation_horaire.pond_retard = LitFlottantCulture(ch[26].Trim());
                                projet.param_affectation_horaire.avance_retard = true;
                            }
                            //MessageBox.Show(p.ToString() + " " + q.ToString() + " " + horaire.ToString());
                            //avancement.textBox1.Text = p.ToString() + " " + q.ToString() + " " + horaire.ToString();
                            //                        avancement.textBox1.Text = flux.Position;
                            //             fich_sortie.WriteLine(pivot.ToString() + projet.reseaux[projet.reseaux].links[pivot].cout.ToString());
                            //                flux.Position += chaine.Length;


                            HashSet<String> filtre = new HashSet<String>();

                            if (projet.param_affectation_horaire.texte_filtre_sortie.Trim().Length > 0)
                            {
                                ch = projet.param_affectation_horaire.texte_filtre_sortie.Split('|');

                                for (int f = 0; f < ch.Length; f++)
                                {
                                    if (filtre.Contains(ch[f].Trim()) == false)

                                        filtre.Add(ch[f].Trim());
                                }
                            }


                            //sens heure de départ//
                            //sens heure de départ//
                            //sens heure de départ//
                            //sens heure de départ//
                            //sens heure de départ//


                            //if (projet.reseaux[projet.reseau_actif].matrices[0].o[p].d.Count > 0)
                            // ---- test de doublon (identique a l'original, evalue sequentiellement) ----
                            ParamSnap snap_cur = od_parallel.Snapshot(projet.param_affectation_horaire);
                            bool skip = false;
                            if (sens == 1)
                            {
                                if (p1 == p && jour1 == jour && horaire1 == horaire && sens1 == sens && nbCols < 13 && snap_cur == snap1)
                                {
                                    q1 = q;
                                    skip = true;
                                }
                                else
                                {
                                    p1 = p; q1 = q; jour1 = jour; horaire1 = horaire; sens1 = sens;
                                }
                            }
                            if (sens == 2)
                            {
                                if (q1 == q && jour1 == jour && horaire1 == horaire && sens1 == sens && nbCols < 13 && snap_cur == snap1)
                                {
                                    p1 = p;
                                    skip = true;
                                }
                                else
                                {
                                    p1 = p; q1 = q; jour1 = jour; horaire1 = horaire; sens1 = sens;
                                }
                            }
                            // ---- envoi de l'OD au calcul parallele ----
                            od_parallel.Submit(new OdTask
                            {
                                p = p, q = q, libod = libod, chaine = chaine, od = od, horaire = horaire,
                                jour = jour, sens = sens, skip = skip, filtre = filtre, pos = posLecture,
                                snap = snap_cur
                            });
                            snap1 = snap_cur;

                        }
                        od_parallel.Finish(); // attend les calculs, ecrit les sorties restantes, cumule volumes/transferts
                        fichier_matrice.Close();
                        flux_matrice.Close();
                        DateTime t2 = DateTime.Now;
                        fich_log.WriteLine("Computation end time: " + t2.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
                        fich_log.WriteLine("Computation duration:" + t2.Subtract(t1).TotalSeconds + " sec");
                        fich_log.Close();

                        fich_sortie.Close();
                        fich_sortie2.Close();
                        fich_od.Close();
                        fich_noeuds.Close();
                        fich_detour.Close();
                        fich_isoles.Close();




                        for (i = 0; i < projet.reseaux[projet.reseau_actif].links.Count; i++)
                        {
                            if (projet.reseaux[projet.reseau_actif].links[i].volau > 0 || projet.reseaux[projet.reseau_actif].links[i].boai > 0 || projet.reseaux[projet.reseau_actif].links[i].alij > 0)
                            {
                                string texte = projet.reseaux[projet.reseau_actif].nodes[projet.reseaux[projet.reseau_actif].links[i].no].i;
                                texte += ";" + projet.reseaux[projet.reseau_actif].nodes[projet.reseaux[projet.reseau_actif].links[i].nd].i + ";" + projet.reseaux[projet.reseau_actif].links[i].ligne.ToString("0");
                                texte += ";" + projet.reseaux[projet.reseau_actif].links[i].volau.ToString("0.00");
                                texte += ";" + projet.reseaux[projet.reseau_actif].links[i].boai.ToString("0.00");
                                texte += ";" + projet.reseaux[projet.reseau_actif].links[i].alij.ToString("0.00");
                                texte += ";" + projet.reseaux[projet.reseau_actif].links[i].texte;
                                texte += ";" + projet.reseaux[projet.reseau_actif].links[i].type;
                                texte += ";" + projet.reseaux[projet.reseau_actif].links[i].toll.ToString("0.000");

                                fich_result.WriteLine(texte);
                            }

                        }


                        fich_result.Close();
                        //projet.reseaux.Remove(projet.reseaux[projet.reseau_actif]);
                        if (projet.param_affectation_horaire.sortie_services == true)
                        {
                            System.IO.StreamWriter fich_services = new System.IO.StreamWriter(projet.param_affectation_horaire.nom_sortie + "_services.txt", false, System.Text.Encoding.UTF8);
                            string texte = "";
                            fich_services.WriteLine("i;j;ligne;service;hd;hf;regime;volau;boia;alij;texte;type");
                            for (i = 0; i < projet.reseaux[projet.reseau_actif].links.Count; i++)
                            {
                                if (projet.reseaux[projet.reseau_actif].links[i].services.Count > 0)
                                {
                                    for (j = 0; j < projet.reseaux[projet.reseau_actif].links[i].services.Count; j++)
                                    {
                                        if (projet.reseaux[projet.reseau_actif].links[i].services[j].volau > 0)
                                        {
                                            texte = projet.reseaux[projet.reseau_actif].nodes[projet.reseaux[projet.reseau_actif].links[i].no].i;
                                            texte += ";" + projet.reseaux[projet.reseau_actif].nodes[projet.reseaux[projet.reseau_actif].links[i].nd].i + ";" + projet.reseaux[projet.reseau_actif].links[i].ligne.ToString("0");
                                            texte += ";" + projet.reseaux[projet.reseau_actif].links[i].services[j].numero;
                                            texte += ";" + projet.reseaux[projet.reseau_actif].links[i].services[j].hd;
                                            texte += ";" + projet.reseaux[projet.reseau_actif].links[i].services[j].hf;
                                            texte += ";" + projet.reseaux[projet.reseau_actif].nom_calendrier[projet.reseaux[projet.reseau_actif].links[i].services[j].regime];
                                            texte += ";" + projet.reseaux[projet.reseau_actif].links[i].services[j].volau.ToString("0.00");
                                            texte += ";" + projet.reseaux[projet.reseau_actif].links[i].services[j].boat.ToString("0.00");
                                            texte += ";" + projet.reseaux[projet.reseau_actif].links[i].services[j].alit.ToString("0.00");
                                            texte += ";" + projet.reseaux[projet.reseau_actif].links[i].texte;
                                            texte += ";" + projet.reseaux[projet.reseau_actif].links[i].type;
                                            fich_services.WriteLine(texte);

                                        }
                                    }
                                }


                            }
                            fich_services.Close();

                        }
                        if (projet.param_affectation_horaire.sortie_turns == true)
                        {
                            System.IO.StreamWriter fich_turns = new System.IO.StreamWriter(projet.param_affectation_horaire.nom_sortie + "_transferts.txt", false, System.Text.Encoding.UTF8);
                            string texte = "";
                            //int k=0;
                            fich_turns.WriteLine("j;i;lignei;k;lignek;textei;textek;volau");

                            foreach (Turn virage in transfers.Keys)
                            {

                                if (transfers[virage] > 0)
                                {
                                    texte = projet.reseaux[projet.reseau_actif].nodes[projet.reseaux[projet.reseau_actif].links[virage.arci].nd].i;

                                    texte += ";" + projet.reseaux[projet.reseau_actif].nodes[projet.reseaux[projet.reseau_actif].links[virage.arci].no].i;
                                    texte += ";" + projet.reseaux[projet.reseau_actif].links[virage.arci].ligne;
                                    texte += ";" + projet.reseaux[projet.reseau_actif].nodes[projet.reseaux[projet.reseau_actif].links[virage.arcj].nd].i;
                                    texte += ";" + projet.reseaux[projet.reseau_actif].links[virage.arcj].ligne;
                                    texte += ";" + projet.reseaux[projet.reseau_actif].links[virage.arci].texte;
                                    texte += ";" + projet.reseaux[projet.reseau_actif].links[virage.arcj].texte;
                                    texte += ";" + transfers[virage];
                                    fich_turns.WriteLine(texte);
                                }










                            }
                            fich_turns.Close();

                        }


                    }


                    //algorithme de Dijkstra

                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra
                    //algorithme de Dijkstra


                    //algorithme de Dijkstra

                    else if (projet.param_affectation_horaire.algorithme == 2)
                    {


                    }
                }


            }
        }

        public class vecteur
        {
            public List<float> d = new List<float>(0);

        }


        public class Turn
        {
            public int arci, arcj;

            public override bool Equals(Object virage)
            {
                if (arci == ((Turn)virage).arci && arcj == ((Turn)virage).arcj)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public override int GetHashCode()
            {
                return unchecked((arci * 73856093) ^ (arcj * 19349663));
            }

        }

        public class turn
        {
            public int numero;
            public float temps;
            public bool is_valid = false;
            //public float volau=0;

        }

        public class node
        {
            public float x, y, tempst = 1e38f, tmap = 0, tatt, temps, cout, ncor, ttoll;
            public string i;
            public bool ci = false, is_valid = true, is_visible = false, is_intersection = false;
            public List<int> pred = new List<int>();
            public List<int> succ = new List<int>();
            public List<int> pred_f, succ_b;   // listes completes (arcs + copies d'etats) pour le sens depart / arrivee
            public List<int> PredAll { get { return pred_f ?? pred; } }
            public List<int> SuccAll { get { return succ_b ?? succ; } }
            //public List<float> ui = new List<float>();
            public string texte;
            public string pole;
        }
        public class Link_num
        {
            public String i, j;
            public int line;
            public override bool Equals(object num_link)
            {
                if (i == ((Link_num)num_link).i && j == ((Link_num)num_link).j && line == ((Link_num)num_link).line)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public override int GetHashCode()
            {
                return (i.GetHashCode() ^ j.GetHashCode() ^ line.GetHashCode());
            }
        }
        public class link
        {
            public link CloneL()
            {
                link c = (link)MemberwiseClone();
                c.services = new List<Service>(services.Count);
                foreach (Service s in services) c.services.Add(s.CloneS());
                return c;
            }
            public float longueur, temps, cout, v0, vsat, tatt, tcor, tveh, tmap, tatt1, a, b, n, volau, lanes, h, l, alij, boai, ncorr, toll = 0, ttoll;
            public int no, nd, service, vdf, touche, pivot, ligne, turn_pivot = -1;//,nb_voies;
            public int orig = -1, sens_clone = 0;   // copie d'arc (etat de sous-chemin) : index de l'arc d'origine, 1 = sens depart, 2 = sens arrivee
            public bool is_queue, is_valid = true;
            //public List<turn> arci = new List<turn>();
            //public List<turn> arcj = new List<turn>();
            //public List<float> ul = new List<float>();
            public List<Service> services = new List<Service>();
            public string texte, modes, pole, type = "0", poleV2;
            /*public float fd(float volau, float len, float precha, float cap, float v0, float a, float b, float n)
            {
                float vc, t0, delay;
                t0 = len / v0;
                vc = (volau + precha) / cap;
                if (vc < 1)
                {
                    delay = t0 * (a - b * vc) / (a - vc);
                }
                else
                {
                    delay = t0 * ((a * (1f - b)) / (n * (a - 1f) * (a - 1f))) * (float)Math.Pow(vc, n) + (n * (a - b) * (a - 1f) - a * (1f - b)) / (n * (a - 1f) * (a - 1f));
                }
                return delay;
            }*/
        }


        public class matrix
        {
            public string nom;
            public List<vecteur> o = new List<vecteur>(0);

        }

        public class network
        {
            public string nom;
            public List<link> links = new List<link>(20000);
            public List<node> nodes = new List<node>(10000);
            public float xl = 1e38f, xu = -1e38f, yl = 1e38f, yu = -1e38f;
            public List<matrix> matrices = new List<matrix>();
            public Dictionary<string, int> numnoeud = new Dictionary<string, int>();
            public Dictionary<string, int> num_calendrier = new Dictionary<string, int>();
            public List<string> nom_calendrier = new List<string>();
            public Dictionary<string, string> noms_arcs = new Dictionary<string, string>();
            public int max_type = 0, nbturns = 0, nbservices = 0;
        }

        public class etude
        {
            public string nom;
            public int reseau_actif;
            public List<network> reseaux = new List<network>();
            public Param_affectation_horaire param_affectation_horaire = new Param_affectation_horaire();

        }

        public class Param_affectation_horaire
        {
            public FastDic fcveh, fcwait, fcmap, fcboa, ftboa, ftboa_max, fcoef_tmap, fctoll;
            public Param_affectation_horaire CloneParam()
            {
                Param_affectation_horaire c = (Param_affectation_horaire)MemberwiseClone();
                c.coef_tmap = new Dictionary<String, float>(coef_tmap);
                c.cmap = new Dictionary<String, float>(cmap);
                c.cwait = new Dictionary<String, float>(cwait);
                c.cboa = new Dictionary<String, float>(cboa);
                c.tboa = new Dictionary<String, float>(tboa);
                c.tboa_max = new Dictionary<String, float>(tboa_max);
                c.cveh = new Dictionary<String, float>(cveh);
                c.ctoll = new Dictionary<String, float>(ctoll);
                c.fcveh = new FastDic(c.cveh); c.fcwait = new FastDic(c.cwait); c.fcmap = new FastDic(c.cmap); c.fcboa = new FastDic(c.cboa);
                c.ftboa = new FastDic(c.tboa); c.ftboa_max = new FastDic(c.tboa_max); c.fcoef_tmap = new FastDic(c.coef_tmap); c.fctoll = new FastDic(c.ctoll);
                return c;
            }
            static bool SameDic(Dictionary<String, float> a, Dictionary<String, float> b)
            {
                if (a.Count != b.Count) return false;
                foreach (KeyValuePair<String, float> kv in a)
                {
                    float v;
                    if (!b.TryGetValue(kv.Key, out v) || v != kv.Value) return false;
                }
                return true;
            }
            // egalite de tous les parametres de calcul/sortie (nb_pop et noms de fichiers exclus)
            public bool SameAs(Param_affectation_horaire o)
            {
                return param_dijkstra == o.param_dijkstra && pu == o.pu && sortie_chemins == o.sortie_chemins && demitours == o.demitours
                    && sortie_services == o.sortie_services && sortie_turns == o.sortie_turns && test_OK == o.test_OK && sortie_noeuds == o.sortie_noeuds
                    && sortie_isoles == o.sortie_isoles && sortie_stops == o.sortie_stops && sortie_temps == o.sortie_temps && algorithme == o.algorithme
                    && max_nb_buckets == o.max_nb_buckets && nb_jours == o.nb_jours && avance_retard == o.avance_retard && pond_avance == o.pond_avance && pond_retard == o.pond_retard && tmapmax == o.tmapmax && temps_max == o.temps_max
                    && texte_coef_tmap == o.texte_coef_tmap && texte_cmap == o.texte_cmap && texte_cwait == o.texte_cwait && texte_cboa == o.texte_cboa
                    && texte_tboa == o.texte_tboa && texte_tboa_max == o.texte_tboa_max && texte_cveh == o.texte_cveh && texte_toll == o.texte_toll
                    && texte_filtre_sortie == o.texte_filtre_sortie
                    && SameDic(coef_tmap, o.coef_tmap) && SameDic(cmap, o.cmap) && SameDic(cwait, o.cwait) && SameDic(cboa, o.cboa)
                    && SameDic(tboa, o.tboa) && SameDic(tboa_max, o.tboa_max) && SameDic(cveh, o.cveh) && SameDic(ctoll, o.ctoll);
            }
            // remet les parametres modifiables par une ligne de matrice a leurs valeurs de reference (fichier de parametres)
            public void RestaurerDepuis(Param_affectation_horaire b)
            {
                param_dijkstra = b.param_dijkstra; pu = b.pu; sortie_chemins = b.sortie_chemins; demitours = b.demitours;
                sortie_services = b.sortie_services; sortie_turns = b.sortie_turns; test_OK = b.test_OK; sortie_noeuds = b.sortie_noeuds;
                sortie_isoles = b.sortie_isoles; sortie_stops = b.sortie_stops; sortie_temps = b.sortie_temps; algorithme = b.algorithme;
                max_nb_buckets = b.max_nb_buckets; nb_jours = b.nb_jours; avance_retard = b.avance_retard; pond_avance = b.pond_avance; pond_retard = b.pond_retard;
                texte_coef_tmap = b.texte_coef_tmap; texte_cmap = b.texte_cmap; texte_cwait = b.texte_cwait; texte_cboa = b.texte_cboa;
                texte_tboa = b.texte_tboa; texte_tboa_max = b.texte_tboa_max; texte_cveh = b.texte_cveh; texte_toll = b.texte_toll;
                tmapmax = b.tmapmax; temps_max = b.temps_max; texte_filtre_sortie = b.texte_filtre_sortie;
                CopieDic(coef_tmap, b.coef_tmap); CopieDic(cmap, b.cmap); CopieDic(cwait, b.cwait); CopieDic(cboa, b.cboa);
                CopieDic(tboa, b.tboa); CopieDic(tboa_max, b.tboa_max); CopieDic(cveh, b.cveh); CopieDic(ctoll, b.ctoll);
            }
            static void CopieDic(Dictionary<String, float> d, Dictionary<String, float> src)
            {
                d.Clear();
                foreach (KeyValuePair<String, float> kv in src) d[kv.Key] = kv.Value;
            }
            public string nom_reseau, nom_matrice, nom_sortie, nom_penalites;
            public Dictionary<String, float> coef_tmap = new Dictionary<String, float>();
            public Dictionary<String, float> cmap = new Dictionary<String, float>();
            public Dictionary<String, float> cwait = new Dictionary<String, float>();
            public Dictionary<String, float> cboa = new Dictionary<String, float>();
            public Dictionary<String, float> tboa = new Dictionary<String, float>();
            public Dictionary<String, float> tboa_max = new Dictionary<String, float>(1);
            public Dictionary<String, float> cveh = new Dictionary<String, float>(1);
            public Dictionary<String, float> ctoll = new Dictionary<String, float>(1);
            public float param_dijkstra, pu;
            public bool sortie_chemins, demitours = true, sortie_services = false, sortie_turns = false, test_OK = false, sortie_noeuds = true, sortie_isoles = false, sortie_stops = false;
            public int sortie_temps;
            public int algorithme = 1;
            public float max_nb_buckets = 10000;
            public int nb_jours = 0;
            public bool avance_retard = false;   // ponderations avance/retard renseignees dans le fichier de parametres
            public float pond_avance = 0, pond_retard = 0;
            public string texte_coef_tmap;
            public string texte_cmap;
            public string texte_cwait;
            public string texte_cboa;
            public string texte_tboa;
            public string texte_tboa_max;
            public string texte_cveh, texte_toll;
            public float tmapmax, temps_max = 120;
            public int nb_pop = 0;
            public string texte_filtre_sortie = "";
        }

        public class Service
        {
            public Service CloneS() { return (Service)MemberwiseClone(); }
            public int numero;
            public float hd, hf, delta = 0, boai = 0, alij = 0, alit = 0, boat = 0, volau = 0;
            public int regime;
        }

        public class suivant
        {
            public List<int> classe = new List<int>();

        }



        public class Node
        {
            public string i = "";
            public float x = 0, y = 0, tempst = 1e38f, tmap = 0, tatt, temps, cout, ncor, ttoll;
            public string name = "";
            public string pole, poleV2;
            public List<int> in_nodes = new List<int>();
            public List<int> out_nodes = new List<int>();
            public void addincoming(int i)
            {
                this.in_nodes.Add(i);
            }
            public void addoutgoing(int i)
            {
                this.out_nodes.Add(i);
            }

        }

        public class Link
        {

        }

        // =====================================================================================
        //  PARALLELISATION DU CALCUL DES OD
        //  - la matrice est lue et interpretee sequentiellement (parametres par ligne, doublons)
        //  - les OD de meme origine/jour/heure (ou destination/jour/heure) forment un "groupe" :
        //    un calcul de plus court chemin + les OD suivantes qui reutilisent le meme arbre
        //  - chaque groupe est calcule par un thread sur sa propre copie des arcs
        //  - les sorties (_od, _temps, _chemins, _noeuds, _detour, _isoles, _log), l'affichage
        //    d'avancement et les cumuls (volau, boai, alij, services, transferts) sont appliques
        //    dans l'ordre d'origine de la matrice par un thread d'ecriture unique
        // =====================================================================================
        // Dictionnaire type d'arc -> valeur, en lecture seule, par balayage de quelques entrees (pas de calcul de hachage)
        public class FastDic
        {
            string[] keys; float[] vals;
            public FastDic(Dictionary<String, float> d)
            {
                keys = new string[d.Count]; vals = new float[d.Count];
                int i = 0;
                foreach (KeyValuePair<String, float> kv in d) { keys[i] = string.Intern(kv.Key); vals[i] = kv.Value; i++; }
            }
            public float this[string k]
            {
                get
                {
                    for (int i = 0; i < keys.Length; i++) if ((object)keys[i] == (object)k) return vals[i];
                    for (int i = 0; i < keys.Length; i++) if (keys[i] == k) return vals[i];
                    throw new KeyNotFoundException("The given key '" + k + "' was not present in the dictionary.");
                }
            }
        }

        // =====================================================================================
        //  SOUS-CHEMINS (suites de n arcs) PENALISES OU INTERDITS
        //  Ligne du fichier de penalites :  P;temps;cout;peage;noeud0;ligne1;noeud1;ligne2;noeud2;...;noeudN
        //  (temps < 0 : sous-chemin interdit ; cout et peage peuvent etre negatifs)
        //  Principe : un automate (Aho-Corasick) suit la correspondance du chemin avec les debuts de sous-chemins.
        //  Chaque etat (profondeur >= 2) est represente par une COPIE de l'arc ; l'arc d'origine porte le cas par defaut.
        //  Les copies sont etiquetees comme n'importe quel arc : le chemin "hors interdiction" reste donc disponible.
        // =====================================================================================
        public class SousChemin
        {
            public int[] arcs;
            public float temps, cout, peage;
        }

        public class EtatsData
        {
            public Dictionary<long, int> redirF = new Dictionary<long, int>(), redirB = new Dictionary<long, int>();
            public List<int>[] cloF, cloB;
            public int nOrig = 0, nEtats = 0;
        }

        class AutoSC
        {
            public List<Dictionary<int, int>> child = new List<Dictionary<int, int>>();
            public List<int> fail = new List<int>(), depth = new List<int>(), last = new List<int>();
            public List<List<int>> outAll = new List<List<int>>();
            public List<int> cloneOf = new List<int>();
            List<List<int>> own = new List<List<int>>();
            int NewNode(int d, int a)
            {
                child.Add(new Dictionary<int, int>()); fail.Add(0); depth.Add(d); last.Add(a); own.Add(new List<int>()); outAll.Add(new List<int>()); cloneOf.Add(-1);
                return child.Count - 1;
            }
            public AutoSC(List<int[]> seqs)
            {
                NewNode(0, -1);
                for (int p = 0; p < seqs.Count; p++)
                {
                    int s = 0;
                    foreach (int a in seqs[p])
                    {
                        int v;
                        if (!child[s].TryGetValue(a, out v)) { v = NewNode(depth[s] + 1, a); child[s][a] = v; }
                        s = v;
                    }
                    own[s].Add(p);
                }
                Queue<int> q = new Queue<int>();
                foreach (KeyValuePair<int, int> kv in child[0]) { fail[kv.Value] = 0; outAll[kv.Value] = new List<int>(own[kv.Value]); q.Enqueue(kv.Value); }
                while (q.Count > 0)
                {
                    int u = q.Dequeue();
                    foreach (KeyValuePair<int, int> kv in child[u])
                    {
                        int a = kv.Key, v = kv.Value, f = fail[u];
                        while (f != 0 && !child[f].ContainsKey(a)) f = fail[f];
                        int g;
                        fail[v] = (child[f].TryGetValue(a, out g) && g != v) ? g : 0;
                        outAll[v] = new List<int>(own[v]); outAll[v].AddRange(outAll[fail[v]]);
                        q.Enqueue(v);
                    }
                }
            }
            public int Step(int s, int a)
            {
                while (s != 0 && !child[s].ContainsKey(a)) s = fail[s];
                int v;
                return child[s].TryGetValue(a, out v) ? v : 0;
            }
            public int Eff(int v)
            {
                while (v != 0 && child[v].Count == 0) v = fail[v];
                return v;
            }
        }

        static float LitPeageCulture(string t) { return LitFlottantCulture(t.Trim()); }

        static int TrouveArc(network r, int ni, int nj, int ligne)
        {
            int res = -1;
            foreach (int idx in r.nodes[ni].succ)
                if (r.links[idx].nd == nj && r.links[idx].ligne == ligne) res = idx;
            return res;
        }

        // Ligne "P;..." du fichier de penalites. Retourne true si la ligne est un sous-chemin (lue ou ignoree).
        static void LitLigneSousChemin(string[] penal, etude projet, List<SousChemin> pats, Dictionary<Turn, float> turns,
            Dictionary<Turn, float> turnsToll, Dictionary<Turn, float> turnsCost, StreamWriter log)
        {
            network r = projet.reseaux[projet.reseau_actif];
            if (penal.Length < 9 || (penal.Length - 4) % 2 != 1) { log.WriteLine("Sub-path ignored (bad number of fields): " + string.Join(";", penal)); return; }
            SousChemin sc = new SousChemin();
            sc.temps = LitFlottantCulture(penal[1].Trim()); sc.cout = LitFlottantCulture(penal[2].Trim()); sc.peage = LitFlottantCulture(penal[3].Trim());
            int k = (penal.Length - 4) / 2;
            int[] noeuds = new int[k + 1]; int[] lignes = new int[k];
            for (int i = 0; i <= k; i++)
            {
                if (!r.numnoeud.TryGetValue(penal[4 + 2 * i].Trim(), out noeuds[i])) { log.WriteLine("Sub-path ignored (unknown node): " + string.Join(";", penal)); return; }
                if (i < k) lignes[i] = int.Parse(penal[5 + 2 * i].Trim());
            }
            sc.arcs = new int[k];
            for (int i = 0; i < k; i++)
            {
                sc.arcs[i] = TrouveArc(r, noeuds[i], noeuds[i + 1], lignes[i]);
                if (sc.arcs[i] < 0) { log.WriteLine("Sub-path ignored (unknown arc): " + string.Join(";", penal)); return; }
            }
            for (int i = 1; i < k; i++) r.nodes[noeuds[i]].is_intersection = true;
            if (k == 2)
            {
                Turn v = new Turn(); v.arci = sc.arcs[0]; v.arcj = sc.arcs[1];
                float dummy;
                if (turns.TryGetValue(v, out dummy) == false)
                {
                    turns.Add(v, sc.temps);
                    if (sc.peage != 0f) turnsToll.Add(v, sc.peage);
                    if (sc.cout != 0f) turnsCost.Add(v, sc.cout);
                }
            }
            else pats.Add(sc);
        }

        // Cree les copies d'arcs (etats) et les tables de redirection / penalites des transitions.
        static EtatsData ConstruitEtats(etude projet, List<SousChemin> pats, Dictionary<Turn, float> turns,
            Dictionary<Turn, float> turnsToll, Dictionary<Turn, float> turnsCost)
        {
            EtatsData ed = new EtatsData();
            network r = projet.reseaux[projet.reseau_actif];
            List<link> links = r.links;
            if (pats.Count == 0) return ed;
            int nO = links.Count; ed.nOrig = nO;
            ed.cloF = new List<int>[nO]; ed.cloB = new List<int>[nO];
            for (int sens = 1; sens <= 2; sens++)
            {
                List<int[]> seqs = new List<int[]>();
                foreach (SousChemin p in pats) { int[] a = (int[])p.arcs.Clone(); if (sens == 2) Array.Reverse(a); seqs.Add(a); }
                AutoSC au = new AutoSC(seqs);
                List<int>[] clo = (sens == 1) ? ed.cloF : ed.cloB;
                // copies d'arcs : un etat par noeud du trie de profondeur >= 2 ayant des fils
                Dictionary<int, int> etatDe = new Dictionary<int, int>();   // arc copie -> etat
                for (int v = 1; v < au.child.Count; v++)
                {
                    if (au.depth[v] >= 2 && au.child[v].Count > 0)
                    {
                        int o = au.last[v];
                        link c = links[o].CloneL(); c.orig = o; c.sens_clone = sens;
                        links.Add(c); int ci = links.Count - 1;
                        au.cloneOf[v] = ci; etatDe[ci] = v; ed.nEtats++;
                        if (clo[o] == null) clo[o] = new List<int>();
                        clo[o].Add(ci);
                        if (sens == 1) { node nd = r.nodes[c.nd]; if (nd.pred_f == null) nd.pred_f = new List<int>(nd.pred); nd.pred_f.Add(ci); }
                        else { node nn = r.nodes[c.no]; if (nn.succ_b == null) nn.succ_b = new List<int>(nn.succ); nn.succ_b.Add(ci); }
                    }
                }
                // pivots a traiter : arcs d'origine qui debutent un sous-chemin + toutes les copies
                List<KeyValuePair<int, int>> pivots = new List<KeyValuePair<int, int>>();   // (arc, etat)
                foreach (KeyValuePair<int, int> kv in au.child[0]) pivots.Add(new KeyValuePair<int, int>(kv.Key, kv.Value));
                foreach (KeyValuePair<int, int> kv in etatDe) pivots.Add(new KeyValuePair<int, int>(kv.Key, kv.Value));
                foreach (KeyValuePair<int, int> pv in pivots)
                {
                    int L = pv.Key, s = pv.Value;
                    int Lo = links[L].orig >= 0 ? links[L].orig : L;
                    List<int> voisins = (sens == 1) ? r.nodes[links[L].nd].succ : r.nodes[links[L].no].pred;
                    foreach (int b in voisins)
                    {
                        int v = au.Step(s, b);
                        int eff = au.Eff(v);
                        int tgt = (au.depth[eff] >= 2) ? au.cloneOf[eff] : b;
                        List<int> outs = au.outAll[v];
                        if (tgt == b && outs.Count == 0 && links[L].orig < 0) continue;
                        // combinaison des penalites : sous-chemins termines + penalite de mouvement (2 arcs) de l'ancien format
                        float t = 0f, c = 0f, pe = 0f; bool interdit = false;
                        foreach (int pi in outs)
                        {
                            SousChemin p = pats[pi];
                            if (p.temps < 0f) interdit = true; else if (p.temps > t) t = p.temps;
                            c += p.cout; pe += p.peage;
                        }
                        Turn kold = new Turn();
                        if (sens == 1) { kold.arci = Lo; kold.arcj = b; } else { kold.arci = b; kold.arcj = Lo; }
                        float vt;
                        if (turns.TryGetValue(kold, out vt)) { if (vt < 0f) interdit = true; else if (vt > t) t = vt; }
                        float vp; if (turnsToll.TryGetValue(kold, out vp)) pe += vp;
                        float vc; if (turnsCost.TryGetValue(kold, out vc)) c += vc;
                        Turn knew = new Turn();
                        if (sens == 1) { knew.arci = L; knew.arcj = tgt; } else { knew.arci = tgt; knew.arcj = L; }
                        if (tgt != b)
                        {
                            long key = ((long)L << 32) | (uint)b;
                            if (sens == 1) ed.redirF[key] = tgt; else ed.redirB[key] = tgt;
                        }
                        if (interdit) { turns[knew] = -1f; }
                        else
                        {
                            if (t != 0f) turns[knew] = t;
                            if (pe != 0f) turnsToll[knew] = pe;
                            if (c != 0f) turnsCost[knew] = c;
                        }
                    }
                }
            }
            return ed;
        }

        static int MeilleurEtat(List<link> links, int i, List<int>[] clo)
        {
            int best = i; float bc = links[i].touche != 0 ? links[i].cout : float.MaxValue;
            if (clo != null && i < clo.Length && clo[i] != null)
                foreach (int c in clo[i]) if (links[c].touche != 0 && links[c].cout < bc) { best = c; bc = links[c].cout; }
            return best;
        }

        public class ParamSnap
        {
            public Param_affectation_horaire prm;
        }

        public class OdTask
        {
            public string p, q, libod, chaine;
            public float od, horaire;
            public int jour, sens;
            public bool skip;
            public HashSet<string> filtre;
            public long pos;
            public ParamSnap snap;
        }

        public class OdGroup
        {
            public long seq;
            public List<OdTask> tasks = new List<OdTask>();
        }

        public class OdResult
        {
            public long seq;
            public string od, sortie, sortie2, noeuds, detour, isoles, log;
            public long[] positions;
            public List<int> lkIdx = new List<int>();
            public List<float> lkVolau = new List<float>(), lkBoai = new List<float>(), lkAlij = new List<float>();
            public List<int> svIdx = new List<int>();   // index global du service (offsets par arc)
            public List<float> svVolau = new List<float>(), svAlit = new List<float>(), svBoat = new List<float>();
            public List<KeyValuePair<Turn, float>> transferts = new List<KeyValuePair<Turn, float>>();
        }

        public class OdWorker
        {
            public etude projet;
            public Dictionary<Turn, float> transfers = new Dictionary<Turn, float>();
            public Dictionary<Turn, float> turnsToll, turnsCost;
            public Dictionary<long, int> redirF, redirB;
            public List<int>[] cloF, cloB;
            public List<List<int>> gga_nq = new List<List<int>>();
            public Turn turnTest = new Turn();
            public ParamSnap lastSnap;
            public Param_affectation_horaire lastPrm;
        }

        public class OdParallel
        {
            etude master;
            Dictionary<Turn, float> turns, turnsToll, turnsCost, transfers;
            EtatsData etats;
            long length;
            int ctop, cleft, avancement;
            StreamWriter w_sortie, w_sortie2, w_od, w_noeuds, w_detour, w_isoles, w_log;
            int nThreads;
            Thread[] workers;
            Thread writer;
            BlockingCollection<OdGroup> queue = new BlockingCollection<OdGroup>();
            SemaphoreSlim window;
            Dictionary<long, OdResult> results = new Dictionary<long, OdResult>();
            long nextSeq = 0, totalGroups = -1;
            OdGroup current;
            ParamSnap lastSnap;
            Exception failure;
            double[] accVolau, accBoai, accAlij, accSvVolau, accSvAlit, accSvBoat;
            int[] svOff;
            Dictionary<Turn, double> trAcc = new Dictionary<Turn, double>();

            public OdParallel(etude projet, Dictionary<Turn, float> turns, Dictionary<Turn, float> turnsToll, Dictionary<Turn, float> turnsCost, EtatsData etats, Dictionary<Turn, float> transfers, long length, int ctop, int cleft, int avancement,
                StreamWriter fich_sortie, StreamWriter fich_sortie2, StreamWriter fich_od, StreamWriter fich_noeuds, StreamWriter fich_detour, StreamWriter fich_isoles, StreamWriter fich_log)
            {
                master = projet; this.turns = turns; this.turnsToll = turnsToll; this.turnsCost = turnsCost; this.etats = etats; this.transfers = transfers; this.length = length;
                this.ctop = ctop; this.cleft = cleft; this.avancement = avancement;
                w_sortie = fich_sortie; w_sortie2 = fich_sortie2; w_od = fich_od; w_noeuds = fich_noeuds; w_detour = fich_detour; w_isoles = fich_isoles; w_log = fich_log;

                List<link> links = master.reseaux[master.reseau_actif].links;
                svOff = new int[links.Count + 1];
                for (int i = 0; i < links.Count; i++) svOff[i + 1] = svOff[i] + links[i].services.Count;
                accVolau = new double[links.Count]; accBoai = new double[links.Count]; accAlij = new double[links.Count];
                accSvVolau = new double[svOff[links.Count]]; accSvAlit = new double[svOff[links.Count]]; accSvBoat = new double[svOff[links.Count]];

                nThreads = Environment.ProcessorCount;
                int env;
                if (int.TryParse(Environment.GetEnvironmentVariable("MUSLIC_THREADS"), out env) && env > 0) nThreads = env;
                if (nThreads < 1) nThreads = 1;
                window = new SemaphoreSlim(Math.Max(16, nThreads * 8));

                CultureInfo culture = CultureInfo.CurrentCulture;
                workers = new Thread[nThreads];
                for (int t = 0; t < nThreads; t++)
                {
                    workers[t] = new Thread(() => WorkerLoop(culture)) { IsBackground = true };
                    workers[t].Start();
                }
                writer = new Thread(() => WriterLoop(culture)) { IsBackground = true };
                writer.Start();
            }

            // instantane des parametres de la ligne (reutilise l'instantane precedent s'il n'a pas change)
            public ParamSnap Snapshot(Param_affectation_horaire prm)
            {
                if (lastSnap != null && lastSnap.prm.SameAs(prm)) return lastSnap;
                lastSnap = new ParamSnap { prm = prm.CloneParam() };
                return lastSnap;
            }

            void Fail(Exception e)
            {
                lock (results)
                {
                    if (failure == null) failure = e;
                    Monitor.PulseAll(results);
                }
                try { queue.CompleteAdding(); } catch (Exception) { }
            }

            void CheckFailure()
            {
                Exception e;
                lock (results) { e = failure; }
                if (e != null) System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(e).Throw();
            }

            void Enqueue(OdGroup g)
            {
                while (!window.Wait(100)) CheckFailure();
                CheckFailure();
                queue.Add(g);
            }

            public void Submit(OdTask t)
            {
                if (!t.skip || current == null)
                {
                    if (current != null) Enqueue(current);
                    current = new OdGroup { seq = nextSeq++ };
                }
                current.tasks.Add(t);
            }

            public void Finish()
            {
                if (current != null) { Enqueue(current); current = null; }
                lock (results) { totalGroups = nextSeq; Monitor.PulseAll(results); }
                queue.CompleteAdding();
                foreach (Thread t in workers) t.Join();
                writer.Join();
                CheckFailure();

                // copies d'arcs (etats de sous-chemins) : on replie leurs cumuls sur les arcs d'origine puis on les retire du reseau
                List<link> links = master.reseaux[master.reseau_actif].links;
                int nO = etats.nOrig;
                if (nO > 0)
                {
                    for (int i = nO; i < links.Count; i++)
                    {
                        int o = links[i].orig;
                        accVolau[o] += accVolau[i]; accBoai[o] += accBoai[i]; accAlij[o] += accAlij[i];
                        for (int j = 0; j < links[i].services.Count; j++)
                        {
                            accSvVolau[svOff[o] + j] += accSvVolau[svOff[i] + j]; accSvAlit[svOff[o] + j] += accSvAlit[svOff[i] + j]; accSvBoat[svOff[o] + j] += accSvBoat[svOff[i] + j];
                        }
                    }
                    Dictionary<Turn, double> tr2 = new Dictionary<Turn, double>();
                    foreach (KeyValuePair<Turn, double> kv in trAcc)
                    {
                        Turn k2 = new Turn();
                        k2.arci = kv.Key.arci >= nO ? links[kv.Key.arci].orig : kv.Key.arci;
                        k2.arcj = kv.Key.arcj >= nO ? links[kv.Key.arcj].orig : kv.Key.arcj;
                        double v2;
                        if (tr2.TryGetValue(k2, out v2)) tr2[k2] = v2 + kv.Value; else tr2[k2] = kv.Value;
                    }
                    trAcc = tr2;
                    links.RemoveRange(nO, links.Count - nO);
                }
                // cumuls dans le reseau principal (utilises par les ecritures finales _aff, _services, _transferts)
                for (int i = 0; i < links.Count; i++)
                {
                    links[i].volau = (float)accVolau[i]; links[i].boai = (float)accBoai[i]; links[i].alij = (float)accAlij[i];
                    for (int j = 0; j < links[i].services.Count; j++)
                    {
                        Service s = links[i].services[j];
                        s.volau = (float)accSvVolau[svOff[i] + j]; s.alit = (float)accSvAlit[svOff[i] + j]; s.boat = (float)accSvBoat[svOff[i] + j];
                    }
                }
                foreach (KeyValuePair<Turn, double> kv in trAcc) transfers[kv.Key] = (float)kv.Value;
            }

            void WorkerLoop(CultureInfo culture)
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                    OdWorker w = NewWorker();
                    foreach (OdGroup g in queue.GetConsumingEnumerable())
                    {
                        CheckFailure();
                        OdResult r = RunGroup(w, g);
                        lock (results) { results[g.seq] = r; Monitor.PulseAll(results); }
                    }
                }
                catch (Exception e) { Fail(e); }
            }

            // copie privee des arcs (les noeuds, la table des noeuds et les calendriers sont partages en lecture seule)
            OdWorker NewWorker()
            {
                network src = master.reseaux[master.reseau_actif];
                network net = new network();
                net.nom = src.nom;
                net.links = new List<link>(src.links.Count);
                foreach (link l in src.links) { link cl = l.CloneL(); if (cl.type != null) cl.type = string.Intern(cl.type); net.links.Add(cl); }
                net.nodes = src.nodes;
                net.numnoeud = src.numnoeud;
                net.num_calendrier = src.num_calendrier;
                net.nom_calendrier = src.nom_calendrier;
                net.noms_arcs = src.noms_arcs;
                net.max_type = src.max_type; net.nbturns = src.nbturns; net.nbservices = src.nbservices;
                OdWorker w = new OdWorker();
                w.turnsToll = turnsToll; w.turnsCost = turnsCost;
                w.redirF = etats.redirF; w.redirB = etats.redirB; w.cloF = etats.cloF; w.cloB = etats.cloB;
                w.projet = new etude();
                w.projet.reseaux.Add(net);
                w.projet.reseau_actif = 0;
                return w;
            }

            OdResult RunGroup(OdWorker w, OdGroup g)
            {
                StringWriter s_od = new StringWriter(), s_sortie = new StringWriter(), s_sortie2 = new StringWriter(), s_noeuds = new StringWriter(),
                    s_detour = new StringWriter(), s_isoles = new StringWriter(), s_log = new StringWriter();
                OdResult r = new OdResult { seq = g.seq, positions = new long[g.tasks.Count] };
                for (int k = 0; k < g.tasks.Count; k++)
                {
                    OdTask t = g.tasks[k];
                    r.positions[k] = t.pos;
                    if (t.snap != w.lastSnap)
                    {
                        w.lastSnap = t.snap;
                        w.lastPrm = t.snap.prm.CloneParam();
                    }
                    w.projet.param_affectation_horaire = w.lastPrm;
                    CalculeOD(t, w, turns, s_sortie, s_sortie2, s_od, s_noeuds, s_detour, s_isoles, s_log);
                }
                r.od = s_od.ToString(); r.sortie = s_sortie.ToString(); r.sortie2 = s_sortie2.ToString(); r.noeuds = s_noeuds.ToString();
                r.detour = s_detour.ToString(); r.isoles = s_isoles.ToString(); r.log = s_log.ToString();

                // recolte des cumuls du groupe (puis remise a zero pour que le groupe suivant parte de zero)
                List<link> links = w.projet.reseaux[0].links;
                for (int i = 0; i < links.Count; i++)
                {
                    link l = links[i];
                    if (l.volau != 0 || l.boai != 0 || l.alij != 0)
                    {
                        r.lkIdx.Add(i); r.lkVolau.Add(l.volau); r.lkBoai.Add(l.boai); r.lkAlij.Add(l.alij);
                        l.volau = 0; l.boai = 0; l.alij = 0;
                    }
                    for (int j = 0; j < l.services.Count; j++)
                    {
                        Service s = l.services[j];
                        if (s.volau != 0 || s.alit != 0 || s.boat != 0)
                        {
                            r.svIdx.Add(svOff[i] + j); r.svVolau.Add(s.volau); r.svAlit.Add(s.alit); r.svBoat.Add(s.boat);
                            s.volau = 0; s.alit = 0; s.boat = 0;
                        }
                    }
                }
                foreach (KeyValuePair<Turn, float> kv in w.transfers) r.transferts.Add(kv);
                w.transfers.Clear();
                return r;
            }

            void WriterLoop(CultureInfo culture)
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                    for (long seq = 0; ; seq++)
                    {
                        OdResult r;
                        lock (results)
                        {
                            while (true)
                            {
                                if (failure != null) return;
                                if (results.TryGetValue(seq, out r)) { results.Remove(seq); break; }
                                if (totalGroups >= 0 && seq >= totalGroups) return;
                                Monitor.Wait(results);
                            }
                        }
                        Ecrit(r);
                        window.Release();
                    }
                }
                catch (Exception e) { Fail(e); }
            }

            void Ecrit(OdResult r)
            {
                // affichage d'avancement identique a l'original (position de lecture de chaque OD)
                foreach (long pos in r.positions)
                {
                    if (avancement < (int)((100 * pos) / length))
                    {
                        Console.SetCursorPosition(cleft, ctop);
                        Console.Write("Shortest paths computing...:" + ((100 * pos) / length).ToString() + "%");
                        avancement = (int)((100 * pos) / length);
                    }
                }
                w_od.Write(r.od); w_sortie.Write(r.sortie); w_sortie2.Write(r.sortie2); w_noeuds.Write(r.noeuds);
                w_detour.Write(r.detour); w_isoles.Write(r.isoles); w_log.Write(r.log);
                for (int k = 0; k < r.lkIdx.Count; k++)
                {
                    accVolau[r.lkIdx[k]] += r.lkVolau[k]; accBoai[r.lkIdx[k]] += r.lkBoai[k]; accAlij[r.lkIdx[k]] += r.lkAlij[k];
                }
                for (int k = 0; k < r.svIdx.Count; k++)
                {
                    accSvVolau[r.svIdx[k]] += r.svVolau[k]; accSvAlit[r.svIdx[k]] += r.svAlit[k]; accSvBoat[r.svIdx[k]] += r.svBoat[k];
                }
                foreach (KeyValuePair<Turn, float> kv in r.transferts)
                {
                    double v;
                    if (trAcc.TryGetValue(kv.Key, out v)) trAcc[kv.Key] = v + kv.Value; else trAcc[kv.Key] = kv.Value;
                }
            }
        }

        static float LitFlottantCulture(string t)
        {
            if (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator == ".") return float.Parse(t.Replace(",", "."));
            return float.Parse(t.Replace(".", ","));
        }

        // Sens depart, premier embarquement : compare le service retenu (depart au plus tot, "en retard" de 'pr' par minute au-dela du seuil)
        // au dernier service partant avant le seuil ("en avance", 'pa' par minute). numNom = service retenu par le calcul normal (-1 si aucun).
        // En sortie : cout = cout total de la meilleure alternative ; alt = true si c'est le depart en avance (idx, delta renseignes).
        static void AvDepart(etude projet, link lk, int jour, float tref, float seuil, float maxEcart, float coutBase,
            float cveh, float cwait, float coutFixe, float pa, float pr, int numNom,
            out bool alt, out int idx, out float delta, out float cout)
        {
            alt = false; idx = numNom; delta = 0; cout = 1e38f;
            if (numNom != -1)
            {
                Service sn = lk.services[numNom];
                float occn = sn.hd + sn.delta * 1440f;
                cout = coutBase + (sn.hf - sn.hd) * cveh + (occn - tref) * cwait + coutFixe + pr * Math.Max(0f, occn - seuil);
            }
            int idxE = -1; float dE = 0, best = 1e38f;
            for (int ii = 0; ii < lk.services.Count; ii++)
            {
                Service sv = lk.services[ii];
                string cal = projet.reseaux[projet.reseau_actif].nom_calendrier[sv.regime];
                int dmax = (int)Math.Floor((seuil - sv.hd) / 1440f);
                int cap = Math.Min(projet.param_affectation_horaire.nb_jours, cal.Length - 1 - jour);
                if (dmax > cap) dmax = cap;
                for (int d = dmax; d >= -jour; d--)
                {
                    float occ = sv.hd + d * 1440f;
                    if (occ >= seuil) continue;
                    if (cal[jour + d] != 'O') continue;
                    float e = seuil - occ;
                    if (e >= maxEcart) break;
                    float cc = coutBase + (sv.hf - sv.hd) * cveh + coutFixe + pa * e;
                    if (cc < best) { best = cc; idxE = ii; dE = d; }
                    break;
                }
            }
            if (idxE != -1 && best < cout) { alt = true; idx = idxE; delta = dE; cout = best; }
        }

        // Sens arrivee, dernier debarquement : compare le service retenu (arrivee au plus tard, "en avance" de 'pa' par minute avant le seuil)
        // au premier service arrivant apres le seuil ("en retard", 'pr' par minute).
        static void AvArrivee(etude projet, link lk, int jour, float tref, float seuil, float maxEcart, float coutBase,
            float cveh, float cwait, float coutFixe, float pa, float pr, int numNom,
            out bool alt, out int idx, out float delta, out float cout)
        {
            alt = false; idx = numNom; delta = 0; cout = 1e38f;
            if (numNom != -1)
            {
                Service sn = lk.services[numNom];
                float occn = sn.hf + sn.delta * 1440f;
                cout = coutBase + (sn.hf - sn.hd) * cveh + (tref - occn) * cwait + coutFixe + pa * Math.Max(0f, seuil - occn);
            }
            int idxL = -1; float dL = 0, best = 1e38f;
            for (int ii = 0; ii < lk.services.Count; ii++)
            {
                Service sv = lk.services[ii];
                string cal = projet.reseaux[projet.reseau_actif].nom_calendrier[sv.regime];
                int dmin = (int)Math.Floor((seuil - sv.hf) / 1440f) + 1;
                if (dmin < -jour) dmin = -jour;
                int cap = Math.Min(projet.param_affectation_horaire.nb_jours, cal.Length - 1 - jour);
                for (int d = dmin; d <= cap; d++)
                {
                    float occ = sv.hf + d * 1440f;
                    if (occ <= seuil) continue;
                    if (cal[jour + d] != 'O') continue;
                    float l = occ - seuil;
                    if (l >= maxEcart) break;
                    float cc = coutBase + (sv.hf - sv.hd) * cveh + coutFixe + pr * l;
                    if (cc < best) { best = cc; idxL = ii; dL = d; }
                    break;
                }
            }
            if (idxL != -1 && best < cout) { alt = true; idx = idxL; delta = dL; cout = best; }
        }

        // Calcul d'une OD : corps ORIGINAL (plus courts chemins + ecriture des resultats de l'OD),
        // execute sur les arcs prives du thread. Les sorties vont dans des tampons (StringWriter).
        static void CalculeOD(OdTask t, OdWorker w, Dictionary<Turn, float> turns,
            StringWriter fich_sortie, StringWriter fich_sortie2, StringWriter fich_od, StringWriter fich_noeuds,
            StringWriter fich_detour, StringWriter fich_isoles, StringWriter fich_log)
        {
            etude projet = w.projet;
            Dictionary<Turn, float> transfers = w.transfers;
            List<List<int>> gga_nq = w.gga_nq;
            int i, j;
            List<link> links = projet.reseaux[projet.reseau_actif].links;
            List<node> nodes = projet.reseaux[projet.reseau_actif].nodes;
            string p = t.p, q = t.q, libod = t.libod, chaine = t.chaine;
            float od = t.od, horaire = t.horaire;
            int jour = t.jour, sens = t.sens;
            bool skip = t.skip;
            HashSet<String> filtre = t.filtre;
            projet.param_affectation_horaire.nb_pop = 0;
            bool av_on = projet.param_affectation_horaire.avance_retard;
            float pa = projet.param_affectation_horaire.pond_avance, pr = projet.param_affectation_horaire.pond_retard;

                            if (sens == 1)
                            {
                                // (test de doublon deplace dans la lecture sequentielle de la matrice : voir task.skip)
                                if (skip) goto fin_gga;
                                for (i = 0; i < links.Count; i++)
                                {
                                    links[i].pole = "-1";
                                    links[i].poleV2 = "";
                                    links[i].touche = 0;
                                    links[i].cout = 0;
                                    links[i].tatt = 0;
                                    links[i].tatt1 = 0;
                                    links[i].tcor = 0;
                                    links[i].ncorr = 0;
                                    links[i].tmap = 0;
                                    links[i].tveh = 0;
                                    links[i].h = 0;
                                    links[i].ttoll = 0;
                                    links[i].l = 0;
                                    for (j = 0; j < links[i].services.Count; j++)
                                    {
                                        links[i].services[j].delta = 0;
                                        links[i].services[j].alij = 0;
                                        links[i].services[j].boai = 0;
                                    }
                                    links[i].pivot = -1;
                                    links[i].turn_pivot = -1;
                                    links[i].service = -1;
                                    links[i].is_queue = false;





                                }
                                gga_nq.Clear();
                                string depart = p;
                                int pivot = -1, value;
                                int successeur, bucket, id_bucket = 0;
                                String succ_type;
                                float penalite = 0, temps_correspondance, max_correspondance;

                                if (projet.reseaux[projet.reseau_actif].numnoeud.TryGetValue(p, out value) == true)
                                {
                                    for (j = 0; j < nodes[projet.reseaux[projet.reseau_actif].numnoeud[depart]].succ.Count; j++)
                                    {
                                        successeur = nodes[projet.reseaux[projet.reseau_actif].numnoeud[depart]].succ[j];
                                        succ_type = links[successeur].type;
                                        max_correspondance = projet.param_affectation_horaire.ftboa_max[succ_type];





                                        if (links[successeur].ligne < 0 && projet.param_affectation_horaire.fcmap[succ_type] > 0 && links[successeur].temps < projet.param_affectation_horaire.tmapmax)
                                        {
                                            bool test_periode = false;

                                            if (links[successeur].services.Count > 0)
                                            {
                                                int decal_jour = (int)Math.Floor(horaire / 1440f);
                                                int kk;
                                                for (kk = 0; kk < links[successeur].services.Count; kk++)
                                                {
                                                    if (decal_jour <= projet.param_affectation_horaire.nb_jours)
                                                    {
                                                        if (projet.reseaux[projet.reseau_actif].nom_calendrier[links[successeur].services[kk].regime][jour + decal_jour] == 'O' && links[successeur].services[kk].hd + 1440f * decal_jour <= horaire && links[successeur].services[kk].hf + 1440f * decal_jour > horaire)
                                                        {
                                                            test_periode = true;
                                                            links[successeur].service = kk;
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                test_periode = true;
                                            }
                                            //touches.Enqueue(successeur);

                                            if (test_periode == true)
                                            {
                                                links[successeur].touche = 1;
                                                links[successeur].cout = links[successeur].temps * projet.param_affectation_horaire.fcoef_tmap[succ_type] * projet.param_affectation_horaire.fcmap[succ_type] + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type];
                                                links[successeur].l = links[successeur].longueur;
                                                links[successeur].tmap = links[successeur].temps * projet.param_affectation_horaire.fcoef_tmap[succ_type];
                                                links[successeur].ttoll = links[successeur].toll;
                                                links[successeur].h = horaire + links[successeur].temps * projet.param_affectation_horaire.fcoef_tmap[succ_type];
                                                links[successeur].pivot = -1;
                                                links[successeur].turn_pivot = -1;
                                                links[successeur].pole = depart;
                                                links[successeur].poleV2 = "";
                                                //bucket = Convert.ToInt32(Math.Min((Math.Pow(links[successeur].cout, 2) / projet.param_affectation_horaire.param_dijkstra)), projet.param_affectation_horaire.max_nb_buckets);
                                                bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[successeur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));

                                                while (bucket >= gga_nq.Count)
                                                {
                                                    gga_nq.Add(new List<int>());
                                                }
                                                gga_nq[bucket].Add(successeur);
                                                projet.param_affectation_horaire.nb_pop++;

                                            }
                                        }
                                        else if (projet.param_affectation_horaire.fcveh[succ_type] > 0)
                                        {
                                            int ii, jj, num_service = -1, h3 = 0, delta, duree_periode;
                                            float h1 = 1e38f, h2 = 1e38f, cout2 = 1e38f;
                                            for (ii = 0; ii < links[successeur].services.Count; ii++)
                                            {
                                                delta = 0;
                                                duree_periode = projet.reseaux[projet.reseau_actif].nom_calendrier[links[successeur].services[ii].regime].Length;
                                                if ((links[successeur].services[ii].hd + links[successeur].services[ii].delta * 1440f < horaire) || projet.reseaux[projet.reseau_actif].nom_calendrier[links[successeur].services[ii].regime][jour] == 'N')
                                                {

                                                    h1 = 1e38f;
                                                    h2 = 1e38f;
                                                    h3 = -1;
                                                    for (jj = jour + 1; jj <= Math.Min(jour + projet.param_affectation_horaire.nb_jours, duree_periode - 1); jj++)
                                                    {
                                                        if (projet.reseaux[projet.reseau_actif].nom_calendrier[links[successeur].services[ii].regime][jj] == 'O' && (links[successeur].services[ii].hd + (-jour + jj) * 24f * 60f < h1))
                                                        {
                                                            h1 = links[successeur].services[ii].hd + (-jour + jj) * 24f * 60f;
                                                            h2 = (-jour + jj);
                                                            h3 = jj;
                                                        }

                                                    }
                                                    if (h3 != -1)
                                                    {
                                                        links[successeur].services[ii].delta = h2;
                                                    }
                                                    else
                                                    {
                                                        delta = -1;
                                                    }


                                                }


                                                if (links[successeur].services[ii].hd + links[successeur].services[ii].delta * 1440f - horaire < max_correspondance)
                                                {
                                                    if (((links[successeur].services[ii].hf - links[successeur].services[ii].hd) * projet.param_affectation_horaire.fcveh[succ_type] + (links[successeur].services[ii].hd + links[successeur].services[ii].delta * 1440f - horaire) * projet.param_affectation_horaire.fcwait[succ_type]) + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type] < cout2 && delta > -1)
                                                    {
                                                        cout2 = (links[successeur].services[ii].hf - links[successeur].services[ii].hd) * projet.param_affectation_horaire.fcveh[succ_type] + (links[successeur].services[ii].hd + links[successeur].services[ii].delta * 1440f - horaire) * projet.param_affectation_horaire.fcwait[succ_type] + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type];
                                                        num_service = ii;

                                                    }
                                                }

                                            }
                                            bool av_set = false, av_alt = false; float av_cout = 0f, av_delta = 0f; int av_idx = -1;
                                                    if (av_on)
                                                    {
                                                        AvDepart(projet, links[successeur], jour, horaire, horaire, max_correspondance, 0f, projet.param_affectation_horaire.fcveh[succ_type], projet.param_affectation_horaire.fcwait[succ_type], links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type], pa, pr, num_service, out av_alt, out av_idx, out av_delta, out av_cout);
                                                        av_set = (av_cout < 1e37f);
                                                        if (av_alt) { num_service = av_idx; links[successeur].services[av_idx].delta = av_delta; }
                                                    }
                                                    if (num_service != -1)
                                            {
                                                links[successeur].service = num_service;
                                                links[successeur].cout = (links[successeur].services[num_service].hf - links[successeur].services[num_service].hd) * projet.param_affectation_horaire.fcveh[succ_type] + (links[successeur].services[num_service].hd + links[successeur].services[num_service].delta * 1440f - horaire) * projet.param_affectation_horaire.fcwait[succ_type] + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type];

                                                links[successeur].touche = 1;

                                                links[successeur].h = links[successeur].services[links[successeur].service].hf + links[successeur].services[links[successeur].service].delta * 1440f;

                                                //                                    links[successeur].tatt = links[successeur].services[links[successeur].service].hd + links[successeur].services[links[successeur].service].delta - links[pivot].h;
                                                links[successeur].tatt = links[successeur].services[links[successeur].service].hd + links[successeur].services[links[successeur].service].delta * 1440f - horaire;
                                                links[successeur].tatt1 = links[successeur].services[links[successeur].service].hd + links[successeur].services[links[successeur].service].delta * 1440f - horaire;

                                                links[successeur].tveh = links[successeur].services[links[successeur].service].hf - links[successeur].services[links[successeur].service].hd;
                                                links[successeur].tcor = 0;
                                                links[successeur].ncorr = 1;
                                                links[successeur].tmap = 0;
                                                links[successeur].ttoll = links[successeur].toll;
                                                links[successeur].l = links[successeur].longueur;
                                                        if (av_set) { links[successeur].cout = av_cout; }
                                                //bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[successeur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[successeur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));

                                                while (bucket >= gga_nq.Count)
                                                {
                                                    gga_nq.Add(new List<int>());
                                                }
                                                gga_nq[bucket].Add(successeur);
                                                projet.param_affectation_horaire.nb_pop++;
                                                //                                touches.Enqueue(successeur);
                                                links[successeur].pivot = -1;
                                                links[successeur].turn_pivot = -1;
                                                links[successeur].pole = nodes[links[successeur].no].i;
                                                links[successeur].poleV2 = nodes[links[successeur].no].i;
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    fich_log.WriteLine("OD error " + libod + ":" + chaine + ": non existing origin node!");
                                }
                                int bucket_cout_max = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(projet.param_affectation_horaire.temps_max / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                //         MessageBox.Show(projet.param_affectation_horaire.algorithme.ToString());
                                while (gga_nq.Count > id_bucket && bucket_cout_max >= id_bucket)
                                {

                                    while (gga_nq[id_bucket].Count == 0)
                                    {
                                        id_bucket++;
                                        if (id_bucket == gga_nq.Count || id_bucket == bucket_cout_max )
                                        {
                                            goto fin_gga;
                                        }
                                    }

                                    if (projet.param_affectation_horaire.algorithme == 0)
                                    {
                                        pivot = gga_nq[id_bucket][0];
                                        gga_nq[id_bucket].RemoveAt(0);
                                    }
                                    else
                                    {
                                        int k, id_pivot = -1; double cout_max = 1e38f;
                                        for (k = 0; k < gga_nq[id_bucket].Count; k++)
                                        {
                                            if (links[gga_nq[id_bucket][k]].cout < cout_max)
                                            {
                                                cout_max = links[gga_nq[id_bucket][k]].cout;
                                                id_pivot = k;
                                            }
                                        }
                                        pivot = gga_nq[id_bucket][id_pivot];
                                        gga_nq[id_bucket].RemoveAt(id_pivot);
                                        links[pivot].touche = 3;
                                    }



                                    //avancement.textBox1.Text = touches.Count.ToString() + " " + calcules.Count.ToString() + " " + links[pivot].cout;
                                    //avancement.textBox1.Refresh();
                                    for (j = 0; j < nodes[links[pivot].nd].succ.Count; j++)
                                    {
                                        link troncon_succ = links[nodes[links[pivot].nd].succ[j]];
                                        link troncon_pivot = links[pivot];
                                        successeur = nodes[links[pivot].nd].succ[j];
                                if (w.redirF.Count > 0) { int rt; if (w.redirF.TryGetValue(((long)pivot << 32) | (uint)successeur, out rt)) { successeur = rt; troncon_succ = links[successeur]; } }
                                        succ_type = links[successeur].type;

                                        if (projet.param_affectation_horaire.demitours == true)
                                        {

                                            if (troncon_pivot.no == troncon_succ.nd)
                                            {
                                                penalite = -1;
                                            }
                                            else
                                            {
                                                penalite = 0;
                                            }
                                        }
                                        else
                                        {
                                            penalite = 0;
                                        }

                                        Turn virage = w.turnTest;
                                        virage.arci = pivot;
                                        virage.arcj = successeur;
                                        float value2;
                                        if (nodes[troncon_pivot.nd].is_intersection == true)
                                        {
                                            if (turns.TryGetValue(virage, out value2) == true)
                                            {
                                                penalite = turns[virage];
                                            }
                                            else
                                            {
                                                penalite = 0;
                                            }
                                        }

                                        float tpen = 0f, tpc = 0f;   // peage du virage (fichier de penalites) et son cout
                                        if (penalite >= 0 && w.turnsToll.Count > 0 && nodes[troncon_pivot.nd].is_intersection == true && w.turnsToll.TryGetValue(virage, out tpen)) tpc = tpen * projet.param_affectation_horaire.fctoll[succ_type];
                                        if (penalite >= 0 && w.turnsCost.Count > 0 && nodes[troncon_pivot.nd].is_intersection == true) { float tcv; if (w.turnsCost.TryGetValue(virage, out tcv)) tpc += tcv; }
                                        if (penalite >= 0)
                                        {
                                            if (penalite > 0)
                                            {
                                                temps_correspondance = penalite + projet.param_affectation_horaire.ftboa[succ_type];  // le temps de la penalite s'ajoute au temps minimal de correspondance (comme en sens arrivee)
                                                max_correspondance = projet.param_affectation_horaire.ftboa_max[succ_type];

                                            }
                                            else
                                            {
                                                temps_correspondance = projet.param_affectation_horaire.ftboa[succ_type];
                                                max_correspondance = projet.param_affectation_horaire.ftboa_max[succ_type];
                                            }
                                            //successeurs touches pour la première fois
                                            if (links[successeur].touche == 0)
                                            {
                                                // successeur marche à pied
                                                if (links[successeur].ligne < 0 && projet.param_affectation_horaire.fcmap[succ_type] > 0 && links[pivot].tmap + links[successeur].temps < projet.param_affectation_horaire.tmapmax)
                                                {
                                                    bool test_periode = false;
                                                    links[successeur].service = -1;
                                                    if (links[successeur].services.Count > 0)
                                                    {
                                                        int decal_jour = (int)(Math.Floor((links[pivot].h + penalite) / 1440f));
                                                        for (int kk = 0; kk < links[successeur].services.Count; kk++)
                                                        {
                                                            if (decal_jour <= projet.param_affectation_horaire.nb_jours)
                                                            {
                                                                if (projet.reseaux[projet.reseau_actif].nom_calendrier[links[successeur].services[kk].regime][jour + decal_jour] == 'O' && links[successeur].services[kk].hd + 1440f * decal_jour <= links[pivot].h + penalite && links[successeur].services[kk].hf + 1440f * decal_jour > links[pivot].h + penalite)
                                                                {
                                                                    test_periode = true;
                                                                    links[successeur].service = kk;
                                                                }
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        test_periode = true;
                                                    }

                                                    if (test_periode == true)
                                                    {
                                                        links[successeur].cout = links[pivot].cout + (links[successeur].temps + penalite) * projet.param_affectation_horaire.fcoef_tmap[succ_type] * projet.param_affectation_horaire.fcmap[succ_type]/*+ links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type]*/;
                                                        links[successeur].h = links[pivot].h + (links[successeur].temps) * projet.param_affectation_horaire.fcoef_tmap[succ_type] + penalite;
                                                        links[successeur].tatt = links[pivot].tatt;
                                                        links[successeur].tatt1 = links[pivot].tatt1;
                                                        links[successeur].tveh = links[pivot].tveh;
                                                        links[successeur].tcor = links[pivot].tcor;

                                                        links[successeur].ncorr = links[pivot].ncorr;
                                                        links[successeur].tmap = links[pivot].tmap + (penalite + links[successeur].temps) * projet.param_affectation_horaire.fcoef_tmap[succ_type];
                                                        links[successeur].ttoll = links[pivot].ttoll + links[successeur].toll;
                                                    if (tpen != 0f || tpc != 0f) { links[successeur].ttoll += tpen; links[successeur].cout += tpc; if (tpc < 0f && links[successeur].cout < links[pivot].cout) links[successeur].cout = links[pivot].cout; }
                                                        links[successeur].touche = 1;

                                                        links[successeur].l = links[pivot].l + links[successeur].longueur;


                                                        //bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[successeur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                        bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[successeur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));

                                                        while (bucket >= gga_nq.Count)
                                                        {
                                                            gga_nq.Add(new List<int>());
                                                        }
                                                        gga_nq[bucket].Add(successeur);
                                                        projet.param_affectation_horaire.nb_pop++;
                                                        //                                        touches.Enqueue(successeur);
                                                        links[successeur].pivot = pivot;
                                                        links[successeur].turn_pivot = j;

                                                        links[successeur].pole = links[pivot].pole;
                                                        if (links[pivot].ligne > 0)
                                                        {
                                                            links[successeur].poleV2 = links[pivot].poleV2 + "|" + nodes[links[successeur].no].i;

                                                        }
                                                        else
                                                        {
                                                            links[successeur].poleV2 = links[pivot].poleV2;
                                                        }
                                                    }
                                                }
                                                //successeur TC même ligne
                                                else if (links[successeur].ligne == links[pivot].ligne && projet.param_affectation_horaire.fcveh[succ_type] > 0 && links[pivot].ligne > 0)
                                                {
                                                    int ii, num_service = -1;
                                                    for (ii = 0; ii < links[successeur].services.Count; ii++)
                                                    {
                                                        if (links[successeur].services[ii].numero == links[pivot].services[links[pivot].service].numero)
                                                        {
                                                            if (links[successeur].services[ii].hd >= links[pivot].services[links[pivot].service].hf)
                                                            {
                                                                num_service = ii;
                                                            }
                                                        }
                                                    }
                                                    //                                                if (num_service != -1 && links[successeur].services[num_service].hd + links[pivot].services[links[pivot].service].delta * 1440f >= links[pivot].h)
                                                    if (num_service != -1 && links[successeur].services[num_service].hd >= links[pivot].services[links[pivot].service].hf)
                                                    {
                                                        links[successeur].service = num_service;
                                                        links[successeur].services[num_service].delta = links[pivot].services[links[pivot].service].delta;
                                                        links[successeur].touche = 1;
                                                        links[successeur].cout = links[pivot].cout + (links[successeur].services[links[successeur].service].hf + links[successeur].services[links[successeur].service].delta * 1440f - links[pivot].h) * projet.param_affectation_horaire.fcveh[succ_type] + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type];
                                                        links[successeur].h = links[successeur].services[links[successeur].service].hf + links[successeur].services[links[successeur].service].delta * 1440f;
                                                        links[successeur].tatt = links[pivot].tatt;
                                                        links[successeur].tatt1 = links[pivot].tatt1;
                                                        links[successeur].tveh = links[pivot].tveh + links[successeur].services[links[successeur].service].hf + links[successeur].services[links[successeur].service].delta * 1440f - links[pivot].h;
                                                        links[successeur].tcor = links[pivot].tcor;
                                                        links[successeur].ncorr = links[pivot].ncorr;
                                                        links[successeur].l = links[pivot].l + links[successeur].longueur;
                                                        links[successeur].tmap = links[pivot].tmap;
                                                        links[successeur].ttoll = links[pivot].ttoll + links[successeur].toll;
                                                    if (tpen != 0f || tpc != 0f) { links[successeur].ttoll += tpen; links[successeur].cout += tpc; if (tpc < 0f && links[successeur].cout < links[pivot].cout) links[successeur].cout = links[pivot].cout; }

                                                        bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[successeur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));

                                                        //bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[successeur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                        while (bucket >= gga_nq.Count)
                                                        {
                                                            gga_nq.Add(new List<int>());
                                                        }
                                                        gga_nq[bucket].Add(successeur);
                                                        projet.param_affectation_horaire.nb_pop++;
                                                        //touches.Enqueue(successeur);
                                                        links[successeur].pivot = pivot;
                                                        links[successeur].turn_pivot = j;
                                                        links[successeur].pole = links[pivot].pole;
                                                        links[successeur].poleV2 = links[pivot].poleV2;
                                                    }
                                                }

                                                //successeur TC lignes différentes
                                                else if (links[successeur].ligne != links[pivot].ligne && projet.param_affectation_horaire.fcveh[succ_type] > 0 && links[successeur].ligne > 0 /*&& links[pivot].ligne > 0*/)
                                                {
                                                    int ii, jj, num_service = -1, h3 = 0, duree_periode, delta;
                                                    float h1 = 1e38f, h2 = 1e38f, cout2 = 1e38f;

                                                    for (ii = 0; ii < links[successeur].services.Count; ii++)
                                                    {
                                                        delta = 0;

                                                        duree_periode = projet.reseaux[projet.reseau_actif].nom_calendrier[links[successeur].services[ii].regime].Length;

                                                        if ((links[successeur].services[ii].hd + links[successeur].services[ii].delta * 1440f < links[pivot].h + temps_correspondance) || projet.reseaux[projet.reseau_actif].nom_calendrier[links[successeur].services[ii].regime][jour] == 'N')
                                                        {

                                                            h1 = 1e38f;
                                                            h2 = 1e38f;
                                                            h3 = -1;
                                                            for (jj = jour + 1; jj <= Math.Min(jour + projet.param_affectation_horaire.nb_jours, duree_periode - 1); jj++)
                                                            {
                                                                if (projet.reseaux[projet.reseau_actif].nom_calendrier[links[successeur].services[ii].regime][jj] == 'O' && (links[successeur].services[ii].hd + (-jour + jj) * 24f * 60f < h1) && (links[successeur].services[ii].hd + (-jour + jj) * 24f * 60f) - temps_correspondance > links[pivot].h)
                                                                {
                                                                    h1 = links[successeur].services[ii].hd + (-jour + jj) * 24f * 60f;
                                                                    h2 = (-jour + jj);
                                                                    h3 = jj;
                                                                }

                                                            }
                                                            if (h3 != -1)
                                                            {
                                                                if (links[successeur].services[ii].delta > h2 || links[successeur].touche == 0)
                                                                {
                                                                    links[successeur].services[ii].delta = h2;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                delta = -1;
                                                            }


                                                        }


                                                        if ((links[successeur].services[ii].hd + links[successeur].services[ii].delta * 1440f < links[pivot].h + max_correspondance) && (links[successeur].services[ii].hd + links[successeur].services[ii].delta * 1440f >= links[pivot].h + temps_correspondance))

                                                        {
                                                            if (links[pivot].cout + (links[successeur].services[ii].hf - links[successeur].services[ii].hd) * projet.param_affectation_horaire.fcveh[succ_type] + (links[successeur].services[ii].hd + links[successeur].services[ii].delta * 1440f - links[pivot].h) * projet.param_affectation_horaire.fcwait[succ_type] + temps_correspondance * projet.param_affectation_horaire.fcboa[succ_type] + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type] < cout2 && delta > -1)
                                                            {
                                                                cout2 = links[pivot].cout + (links[successeur].services[ii].hf - links[successeur].services[ii].hd) * projet.param_affectation_horaire.fcveh[succ_type] + (links[successeur].services[ii].hd + links[successeur].services[ii].delta * 1440f - links[pivot].h) * projet.param_affectation_horaire.fcwait[succ_type] + temps_correspondance * projet.param_affectation_horaire.fcboa[succ_type] + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type];
                                                                num_service = ii;

                                                            }
                                                        }

                                                    }
                                                    bool av_set = false, av_alt = false; float av_cout = 0f, av_delta = 0f; int av_idx = -1;
                                                    if (av_on && links[pivot].ncorr == 0)
                                                    {
                                                        AvDepart(projet, links[successeur], jour, links[pivot].h, links[pivot].h + temps_correspondance, max_correspondance, links[pivot].cout, projet.param_affectation_horaire.fcveh[succ_type], projet.param_affectation_horaire.fcwait[succ_type], temps_correspondance * projet.param_affectation_horaire.fcboa[succ_type] + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type], pa, pr, num_service, out av_alt, out av_idx, out av_delta, out av_cout);
                                                        av_set = (av_cout < 1e37f);
                                                        if (av_alt) { num_service = av_idx; links[successeur].services[av_idx].delta = av_delta; }
                                                    }
                                                    if (num_service != -1)
                                                    {
                                                        links[successeur].service = num_service;
                                                        links[successeur].cout = links[pivot].cout + (links[successeur].services[num_service].hf - links[successeur].services[num_service].hd) * projet.param_affectation_horaire.fcveh[succ_type] + (links[successeur].services[num_service].hd + links[successeur].services[num_service].delta * 1440f - links[pivot].h) * projet.param_affectation_horaire.fcwait[succ_type] + (temps_correspondance * projet.param_affectation_horaire.fcboa[succ_type]) + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type];

                                                        links[successeur].touche = 1;

                                                        links[successeur].h = links[successeur].services[links[successeur].service].hf + links[successeur].services[links[successeur].service].delta * 1440f;
                                                        if (links[pivot].ncorr == 0)
                                                        {
                                                            links[successeur].tatt1 = links[successeur].services[links[successeur].service].hd + links[successeur].services[links[successeur].service].delta * 1440f - links[pivot].h;
                                                        }
                                                        else
                                                        {
                                                            links[successeur].tatt1 = links[pivot].tatt1;
                                                        }


                                                        links[successeur].tatt = links[pivot].tatt + links[successeur].services[links[successeur].service].hd + links[successeur].services[links[successeur].service].delta * 1440f - links[pivot].h;
                                                        links[successeur].tveh = links[pivot].tveh + links[successeur].services[links[successeur].service].hf - links[successeur].services[links[successeur].service].hd;
                                                        links[successeur].tcor = links[pivot].tcor + temps_correspondance;
                                                        links[successeur].ncorr = links[pivot].ncorr + 1;
                                                        links[successeur].l = links[pivot].l + links[successeur].longueur;
                                                        links[successeur].tmap = links[pivot].tmap;
                                                        links[successeur].ttoll = links[pivot].ttoll + links[successeur].toll;
                                                        if (av_set) { links[successeur].cout = av_cout; }
                                                    if (tpen != 0f || tpc != 0f) { links[successeur].ttoll += tpen; links[successeur].cout += tpc; if (tpc < 0f && links[successeur].cout < links[pivot].cout) links[successeur].cout = links[pivot].cout; }

                                                        //bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[successeur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                        bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[successeur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                        while (bucket >= gga_nq.Count)
                                                        {
                                                            gga_nq.Add(new List<int>());
                                                        }
                                                        gga_nq[bucket].Add(successeur);
                                                        projet.param_affectation_horaire.nb_pop++;
                                                        links[successeur].pivot = pivot;
                                                        links[successeur].turn_pivot = j;

                                                        if (links[pivot].ligne > 0)
                                                        {
                                                            links[successeur].poleV2 = links[pivot].poleV2 + "|" + nodes[links[successeur].no].i + "|" + nodes[links[successeur].no].i;
                                                        }
                                                        else
                                                        {
                                                            links[successeur].poleV2 = links[pivot].poleV2 + "|" + nodes[links[successeur].no].i;
                                                        }

                                                        if (links[pivot].pole == depart)
                                                        {
                                                            links[successeur].pole = nodes[links[successeur].no].i;
                                                        }
                                                        else
                                                        {
                                                            links[successeur].pole = links[pivot].pole;
                                                        }
                                                        /* if (links[successeur].tveh < 0)
                                                         {
                                                             //fich_sortie.WriteLine("30 " + pivot.ToString() + " " + links[pivot].cout.ToString() + " " + links[successeur].cout.ToString() + " " + links[pivot].ligne.ToString() + " " + links[successeur].ligne.ToString() + " " + links[pivot].h.ToString() + " " + links[successeur].h.ToString());
                                                         }*/
                                                    }
                                                }
                                            }


                                            //eléments déjà touchés
                                            else if (links[successeur].touche == 1 || links[successeur].touche == 2)
                                            {
                                                int id_service = -1;
                                                //bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[successeur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[successeur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                //successeurs marche à pied
                                                if (links[successeur].ligne < 0 && projet.param_affectation_horaire.fcmap[succ_type] > 0 && links[pivot].tmap + links[successeur].temps < projet.param_affectation_horaire.tmapmax)
                                                {
                                                    bool test_periode = false;

                                                    if (links[successeur].services.Count > 0)
                                                    {
                                                        int decal_jour = (int)(Math.Floor((links[pivot].h + penalite) / 1440f));
                                                        for (int kk = 0; kk < links[successeur].services.Count; kk++)
                                                        {
                                                            if (decal_jour <= projet.param_affectation_horaire.nb_jours)
                                                            {
                                                                if (projet.reseaux[projet.reseau_actif].nom_calendrier[links[successeur].services[kk].regime][jour + decal_jour] == 'O' && links[successeur].services[kk].hd + 1440f * decal_jour <= links[pivot].h + penalite && links[successeur].services[kk].hf + 1440f * decal_jour > links[pivot].h + penalite)
                                                                {
                                                                    test_periode = true;
                                                                    id_service = kk;

                                                                }
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        test_periode = true;
                                                    }
                                                    if (test_periode == true)
                                                    {

                                                        if (links[successeur].cout - tpc > links[pivot].cout + (penalite + links[successeur].temps) * projet.param_affectation_horaire.fcoef_tmap[succ_type] * projet.param_affectation_horaire.fcmap[succ_type] + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type])
                                                        {
                                                            bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[successeur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                            gga_nq[bucket].Remove(successeur);
                                                            links[successeur].cout = links[pivot].cout + (links[successeur].temps + penalite) * projet.param_affectation_horaire.fcoef_tmap[succ_type] * projet.param_affectation_horaire.fcmap[succ_type] + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type];
                                                            links[successeur].h = links[pivot].h + (links[successeur].temps) * projet.param_affectation_horaire.fcoef_tmap[succ_type] + penalite;
                                                            links[successeur].tatt = links[pivot].tatt;
                                                            links[successeur].tatt1 = links[pivot].tatt1;
                                                            links[successeur].tveh = links[pivot].tveh;
                                                            links[successeur].tcor = links[pivot].tcor;
                                                            links[successeur].ncorr = links[pivot].ncorr;
                                                            links[successeur].tmap = links[pivot].tmap + (penalite + links[successeur].temps) * projet.param_affectation_horaire.fcoef_tmap[succ_type];
                                                            links[successeur].ttoll = links[pivot].ttoll + links[successeur].toll;
                                                    if (tpen != 0f || tpc != 0f) { links[successeur].ttoll += tpen; links[successeur].cout += tpc; if (tpc < 0f && links[successeur].cout < links[pivot].cout) links[successeur].cout = links[pivot].cout; }

                                                            links[successeur].touche = 2;
                                                            links[successeur].l = links[pivot].l + links[successeur].longueur;
                                                            links[successeur].pivot = pivot;
                                                            links[successeur].turn_pivot = j;
                                                            links[successeur].pole = links[pivot].pole;


                                                            if (links[pivot].ligne > 0)
                                                            {
                                                                links[successeur].poleV2 = links[pivot].poleV2 + "|" + nodes[links[successeur].no].i;

                                                            }
                                                            else
                                                            {
                                                                links[successeur].poleV2 = links[pivot].poleV2;
                                                            }


                                                            links[successeur].service = id_service;                                                        //bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[successeur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), ;
                                                            bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[successeur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                            gga_nq[bucket].Add(successeur);
                                                            projet.param_affectation_horaire.nb_pop++;
                                                        }
                                                    }

                                                }
                                                //successeurs TC même ligne
                                                else if ((links[successeur].ligne == links[pivot].ligne && projet.param_affectation_horaire.fcveh[succ_type] > 0 && links[pivot].ligne > 0 && links[successeur].cout > links[pivot].cout))
                                                {
                                                    int ii, num_service = -1;
                                                    for (ii = 0; ii < links[successeur].services.Count; ii++)
                                                    {
                                                        if (links[successeur].services[ii].numero == links[pivot].services[links[pivot].service].numero)
                                                        {
                                                            if (links[successeur].services[ii].hd >= links[pivot].services[links[pivot].service].hf)
                                                            {
                                                                num_service = ii;
                                                            }
                                                        }


                                                    }

                                                    if (num_service != -1)
                                                    {
                                                        //                                                    if (links[successeur].services[num_service].hd + links[pivot].services[links[pivot].service].delta * 1440f >= links[pivot].h)
                                                        if (links[successeur].services[num_service].hd >= links[pivot].services[links[pivot].service].hf)
                                                        {

                                                            if (links[successeur].cout - tpc > links[pivot].cout + (links[successeur].services[num_service].hf + links[pivot].services[links[pivot].service].delta * 1440f - links[pivot].h) * projet.param_affectation_horaire.fcveh[succ_type] + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type] && links[successeur].services[num_service].hd >= links[pivot].services[links[pivot].service].hf)
                                                            {
                                                                bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[successeur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                                gga_nq[bucket].Remove(successeur);
                                                                links[successeur].services[num_service].delta = links[pivot].services[links[pivot].service].delta;
                                                                links[successeur].service = num_service;
                                                                links[successeur].touche = 2;
                                                                links[successeur].cout = links[pivot].cout + (links[successeur].services[links[successeur].service].hf + links[successeur].services[links[successeur].service].delta * 1440f - links[pivot].h) * projet.param_affectation_horaire.fcveh[succ_type] + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type];
                                                                links[successeur].h = links[successeur].services[links[successeur].service].hf + links[successeur].services[links[successeur].service].delta * 1440f;
                                                                links[successeur].tatt = links[pivot].tatt;
                                                                links[successeur].tatt1 = links[pivot].tatt1;
                                                                links[successeur].tveh = links[pivot].tveh + links[successeur].services[links[successeur].service].hf /* + links[successeur].services[links[successeur].service].delta * 1440f */- links[pivot].h;
                                                                links[successeur].tcor = links[pivot].tcor;
                                                                links[successeur].ncorr = links[pivot].ncorr;
                                                                links[successeur].l = links[pivot].l + links[successeur].longueur;
                                                                links[successeur].tmap = links[pivot].tmap;
                                                                links[successeur].ttoll = links[pivot].ttoll + links[successeur].toll;
                                                    if (tpen != 0f || tpc != 0f) { links[successeur].ttoll += tpen; links[successeur].cout += tpc; if (tpc < 0f && links[successeur].cout < links[pivot].cout) links[successeur].cout = links[pivot].cout; }

                                                                links[successeur].pivot = pivot;

                                                                links[successeur].turn_pivot = j;
                                                                links[successeur].pole = links[pivot].pole;
                                                                links[successeur].poleV2 = links[pivot].poleV2;
                                                                //bucket = Convert.ToInt32(Math.Min((Math.Pow(links[successeur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                                bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[successeur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                                gga_nq[bucket].Add(successeur);
                                                                projet.param_affectation_horaire.nb_pop++;
                                                            }
                                                        }
                                                    }
                                                }
                                                //successeurs TC lignes différentes
                                                else if ((links[successeur].ligne != links[pivot].ligne) && /*links[successeur].ligne>0 && links[pivot].ligne > 0 && */projet.param_affectation_horaire.fcveh[succ_type] > 0 && links[successeur].cout > links[pivot].cout)//&& (links[pivot].h + projet.param_affectation_horaire.tboa < links[successeur].services[links[successeur].service].hd + links[successeur].services[links[successeur].service].delta*1440f))
                                                {
                                                    int ii, jj, num_service = -1, h3 = -1, duree_periode, delta;
                                                    float h1 = 1e38f, h2 = 1e38f, cout2 = 1e38f;
                                                    for (ii = 0; ii < links[successeur].services.Count; ii++)
                                                    {
                                                        delta = 0;
                                                        //links[successeur].services[ii].delta = 0;
                                                        duree_periode = projet.reseaux[projet.reseau_actif].nom_calendrier[links[successeur].services[ii].regime].Length;

                                                        if ((links[successeur].services[ii].hd + links[successeur].services[ii].delta * 1440f < links[pivot].h + temps_correspondance) || projet.reseaux[projet.reseau_actif].nom_calendrier[links[successeur].services[ii].regime][jour] == 'N')
                                                        {

                                                            h1 = 1e38f;
                                                            h2 = 1e38f;
                                                            h3 = -1;
                                                            for (jj = jour + 1; jj <= Math.Min(jour + projet.param_affectation_horaire.nb_jours, duree_periode - 1); jj++)
                                                            {
                                                                if (projet.reseaux[projet.reseau_actif].nom_calendrier[links[successeur].services[ii].regime][jj] == 'O' && (links[successeur].services[ii].hd + (-jour + jj) * 24f * 60f) < h1 && (links[successeur].services[ii].hd + (-jour + jj) * 24f * 60f - temps_correspondance) > links[pivot].h)
                                                                {
                                                                    h1 = links[successeur].services[ii].hd + (-jour + jj) * 24f * 60f;
                                                                    h2 = (-jour + jj);
                                                                    h3 = jj;
                                                                }

                                                            }
                                                            if (h3 != -1)
                                                            {
                                                                if (links[successeur].services[ii].delta < h2 || links[successeur].touche == 0)
                                                                {
                                                                    links[successeur].services[ii].delta = h2;
                                                                }


                                                            }
                                                            else
                                                            {
                                                                delta = -1;
                                                            }


                                                        }
                                                        if ((links[successeur].services[ii].hd + links[successeur].services[ii].delta * 1440f < links[pivot].h + max_correspondance) && (links[successeur].services[ii].hd + links[successeur].services[ii].delta * 1440f >= links[pivot].h + temps_correspondance))
                                                        {
                                                            if (links[pivot].cout + (links[successeur].services[ii].hf - links[successeur].services[ii].hd) * projet.param_affectation_horaire.fcveh[succ_type] + (links[successeur].services[ii].hd + links[successeur].services[ii].delta * 1440f - links[pivot].h) * projet.param_affectation_horaire.fcwait[succ_type] + (temps_correspondance * projet.param_affectation_horaire.fcboa[succ_type]) + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type] < cout2 && delta > -1)
                                                            {
                                                                cout2 = links[pivot].cout + (links[successeur].services[ii].hf - links[successeur].services[ii].hd) * projet.param_affectation_horaire.fcveh[succ_type] + (links[successeur].services[ii].hd + links[successeur].services[ii].delta * 1440f - links[pivot].h) * projet.param_affectation_horaire.fcwait[succ_type] + (temps_correspondance * projet.param_affectation_horaire.fcboa[succ_type]) + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type];
                                                                num_service = ii;
                                                            }
                                                        }

                                                    }
                                                    bool av_set = false, av_alt = false; float av_cout = 0f, av_delta = 0f; int av_idx = -1;
                                                    if (av_on && links[pivot].ncorr == 0)
                                                    {
                                                        AvDepart(projet, links[successeur], jour, links[pivot].h, links[pivot].h + temps_correspondance, max_correspondance, links[pivot].cout, projet.param_affectation_horaire.fcveh[succ_type], projet.param_affectation_horaire.fcwait[succ_type], temps_correspondance * projet.param_affectation_horaire.fcboa[succ_type] + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type], pa, pr, num_service, out av_alt, out av_idx, out av_delta, out av_cout);
                                                        av_set = (av_cout < 1e37f);
                                                        if (av_alt) { num_service = av_idx; links[successeur].services[av_idx].delta = av_delta; }
                                                    }
                                                    if (num_service != -1)
                                                    {
                                                        if (links[successeur].cout - tpc > (av_set ? av_cout : (links[pivot].cout + (links[successeur].services[num_service].hf - links[successeur].services[num_service].hd) * projet.param_affectation_horaire.fcveh[succ_type] + (links[successeur].services[num_service].hd + links[successeur].services[num_service].delta * 1440f - links[pivot].h) * projet.param_affectation_horaire.fcwait[succ_type] + (temps_correspondance * projet.param_affectation_horaire.fcboa[succ_type]) + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type])))
                                                        {
                                                            bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[successeur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                            gga_nq[bucket].Remove(successeur);
                                                            links[successeur].service = num_service;
                                                            links[successeur].cout = links[pivot].cout + (links[successeur].services[num_service].hf - links[successeur].services[num_service].hd) * projet.param_affectation_horaire.fcveh[succ_type] + (links[successeur].services[num_service].hd + links[successeur].services[num_service].delta * 1440f - links[pivot].h) * projet.param_affectation_horaire.fcwait[succ_type] + (temps_correspondance * projet.param_affectation_horaire.fcboa[succ_type]) + links[successeur].toll * projet.param_affectation_horaire.fctoll[succ_type];
                                                            links[successeur].touche = 2;

                                                            links[successeur].h = links[successeur].services[links[successeur].service].hf + links[successeur].services[links[successeur].service].delta * 1440f;
                                                            if (links[pivot].ncorr == 0)
                                                            {
                                                                links[successeur].tatt1 = links[successeur].services[links[successeur].service].hd + links[successeur].services[links[successeur].service].delta * 1440f - links[pivot].h;
                                                            }
                                                            else
                                                            {
                                                                links[successeur].tatt1 = links[pivot].tatt1;
                                                            }
                                                            links[successeur].tatt = links[pivot].tatt + links[successeur].services[links[successeur].service].hd + links[successeur].services[links[successeur].service].delta * 1440f - links[pivot].h;
                                                            links[successeur].tveh = links[pivot].tveh + links[successeur].services[links[successeur].service].hf - links[successeur].services[links[successeur].service].hd;
                                                            links[successeur].tcor = links[pivot].tcor + temps_correspondance;
                                                            links[successeur].ncorr = links[pivot].ncorr + 1;
                                                            links[successeur].l = links[pivot].l + links[successeur].longueur;
                                                            links[successeur].tmap = links[pivot].tmap;
                                                            links[successeur].ttoll = links[pivot].ttoll + links[successeur].toll;
                                                        if (av_set) { links[successeur].cout = av_cout; }
                                                    if (tpen != 0f || tpc != 0f) { links[successeur].ttoll += tpen; links[successeur].cout += tpc; if (tpc < 0f && links[successeur].cout < links[pivot].cout) links[successeur].cout = links[pivot].cout; }

                                                            links[successeur].pivot = pivot;
                                                            links[successeur].turn_pivot = j;
                                                            if (links[pivot].pole == depart)
                                                            {
                                                                links[successeur].pole = nodes[links[successeur].no].i;
                                                            }
                                                            else
                                                            {
                                                                links[successeur].pole = links[pivot].pole;
                                                            }
                                                            if (links[pivot].ligne > 0)
                                                            {

                                                                links[successeur].poleV2 = links[pivot].poleV2 + "|" + nodes[links[successeur].no].i + "|" + nodes[links[successeur].no].i;
                                                            }
                                                            else
                                                            {

                                                                links[successeur].poleV2 = links[pivot].poleV2 + "|" + nodes[links[successeur].no].i;
                                                            }


                                                            bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[successeur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                            gga_nq[bucket].Add(successeur);
                                                            projet.param_affectation_horaire.nb_pop++;
                                                        }
                                                    }
                                                }


                                            }
                                        }
                                    }
                                    //links[pivot].touche = 3;
                                    //Console.WriteLine((touches.Count+calcules.Count).ToString());
                                }
                            fin_gga:
                                //Console.WriteLine(p.ToString());

                                int arrivee = -1;
                                double cout_fin = 1e38f;
                                if (projet.reseaux[projet.reseau_actif].numnoeud.TryGetValue(q, out value) == true)
                                {
                                    for (j = 0; j < nodes[projet.reseaux[projet.reseau_actif].numnoeud[q]].PredAll.Count; j++)
                                    {
                                        int predecesseur = nodes[projet.reseaux[projet.reseau_actif].numnoeud[q]].PredAll[j];
                                        if (links[predecesseur].touche != 0 && links[predecesseur].cout < cout_fin)
                                        {
                                            arrivee = predecesseur;
                                            cout_fin = links[predecesseur].cout;

                                        }




                                    }
                                }
                                else
                                {
                                    fich_log.WriteLine("OD error" + libod + ":" + chaine + ": non existing destination node!");
                                }



                                if (arrivee != -1)
                                {
                                    if (links[arrivee].ligne > 0)
                                    {
                                        links[arrivee].alij += od;
                                        links[arrivee].services[links[arrivee].service].alij = od;
                                        links[arrivee].services[links[arrivee].service].alit += od;
                                    }
                                }
                                else
                                {
                                    fich_log.WriteLine("OD error" + libod + ":" + chaine + ": unreachable destination!");
                                }

                                pivot = arrivee;
                                string itineraire = "", texte;
                                if (pivot != -1)
                                {
                                    string[] param2 = { "|" }, lignes_corr = null;
                                    if (links[pivot].texte != null)
                                    {
                                        lignes_corr = links[pivot].texte.Split(param2, StringSplitOptions.RemoveEmptyEntries);
                                    }
                                    if (lignes_corr == null)
                                    {
                                        itineraire = "MAP";
                                    }
                                    else
                                    {
                                        itineraire = lignes_corr[0];
                                    }
                                }
                                while (pivot != -1)
                                {
                                    links[pivot].volau += od;
                                    if (links[pivot].pivot != -1 && projet.param_affectation_horaire.sortie_turns == true)
                                    {
                                        Turn virage = new Turn();
                                        virage.arci = links[pivot].pivot;
                                        virage.arcj = pivot;
                                        float value2;
                                        if (transfers.TryGetValue(virage, out value2) == true)
                                        {

                                            transfers[virage] += od;
                                        }
                                        else
                                        {
                                            transfers[virage] = od;
                                        }

                                        //links[links[pivot].pivot].arcj[links[pivot].turn_pivot].volau += od;
                                    }
                                    if (links[pivot].service >= 0)
                                    {
                                        links[pivot].services[links[pivot].service].volau += od;
                                    }

                                    if (links[pivot].pivot == -1)
                                    {
                                        if (links[pivot].ligne > 0)
                                        {
                                            links[pivot].boai += od;
                                            links[pivot].services[links[pivot].service].boai = od;
                                            links[pivot].services[links[pivot].service].boat += od;

                                        }
                                    }
                                    else if (links[pivot].ligne != links[links[pivot].pivot].ligne)
                                    {
                                        if (links[pivot].ligne > 0)
                                        {
                                            links[pivot].boai += od;
                                            links[pivot].services[links[pivot].service].boai = od;
                                            links[pivot].services[links[pivot].service].boat += od;
                                        }
                                        if (links[links[pivot].pivot].ligne > 0)
                                        {
                                            links[links[pivot].pivot].alij += od;
                                            links[links[pivot].pivot].services[links[links[pivot].pivot].service].alij = od;
                                            links[links[pivot].pivot].services[links[links[pivot].pivot].service].alit += od;
                                        }

                                    }
                                    if (projet.param_affectation_horaire.sortie_chemins == true)
                                    {
                                        texte = libod + ";" + p + ";" + q + ";" + jour.ToString("0") + ";" + horaire.ToString("0.000");

                                        texte += ";" + nodes[links[pivot].no].i;
                                        texte += ";" + nodes[links[pivot].nd].i;
                                        texte += ";" + nodes[links[pivot].no].i + "-" + nodes[links[pivot].nd].i;
                                        texte += ";" + links[pivot].ligne.ToString("0");
                                        if (links[pivot].service >= 0)
                                        {
                                            texte += ";" + links[pivot].services[links[pivot].service].numero.ToString("0");
                                        }
                                        else
                                        {
                                            texte += ";-1";
                                        }
                                        texte += ";" + (links[pivot].h - horaire).ToString("0.000");
                                        texte += ";" + links[pivot].h.ToString("0.000");
                                        texte += ";" + links[pivot].tveh.ToString("0.000");
                                        texte += ";" + links[pivot].tmap.ToString("0.000");
                                        texte += ";" + links[pivot].tatt.ToString("0.000");
                                        texte += ";" + links[pivot].tcor.ToString("0.000");
                                        texte += ";" + links[pivot].ncorr.ToString("0");
                                        texte += ";" + links[pivot].tatt1.ToString("0.000");
                                        texte += ";" + links[pivot].cout.ToString("0.000");
                                        texte += ";" + links[pivot].l.ToString("0.000");
                                        texte += ";" + links[pivot].pole;
                                        texte += ";" + od.ToString("0.00");
                                        if (links[pivot].ligne > 0)
                                        {
                                            texte += ";" + links[pivot].services[links[pivot].service].boai.ToString("0.000");
                                            texte += ";" + links[pivot].services[links[pivot].service].alij.ToString("0.000");
                                            links[pivot].services[links[pivot].service].boai = 0;
                                            links[pivot].services[links[pivot].service].alij = 0;

                                        }
                                        else
                                        {
                                            texte += ";0.000";
                                            texte += ";0.000";
                                        }

                                        texte += ";" + links[pivot].texte;
                                        texte += ";" + links[pivot].type;

                                        texte += ";" + links[pivot].ttoll.ToString("0.000");

                                        fich_sortie2.WriteLine(texte);


                                    }
                                    if (links[pivot].pivot != -1)
                                    {
                                        if (links[pivot].ligne != links[links[pivot].pivot].ligne)
                                        {
                                            string[] param2 = { "|" }, lignes_corr = null;
                                            if (links[pivot].texte != null)
                                            {

                                                lignes_corr = links[links[pivot].pivot].texte.Split(param2, StringSplitOptions.RemoveEmptyEntries);
                                            }
                                            if (lignes_corr == null)
                                            {
                                                itineraire = "MAP|" + itineraire; ;
                                            }
                                            else
                                            {
                                                itineraire = lignes_corr[0] + "|" + itineraire;
                                            }
                                        }
                                    }
                                    pivot = links[pivot].pivot;
                                }
                                //fich_sortie.WriteLine("o;i;j;jour;heureo;heured;temps;tveh;tmap;tcorr;cout;volau;texte" );
                                if (arrivee != -1)
                                {
                                    texte = libod + ";" + p + ";" + q;
                                    texte += ";" + jour.ToString("0.000");
                                    texte += ";" + horaire.ToString("0.000");
                                    texte += ";" + links[arrivee].h.ToString("0.000");
                                    texte += ";" + (-horaire + links[arrivee].h).ToString("0.000");
                                    texte += ";" + links[arrivee].tveh.ToString("0.000");
                                    texte += ";" + links[arrivee].tmap.ToString("0.000");
                                    texte += ";" + links[arrivee].tatt.ToString("0.000");
                                    texte += ";" + links[arrivee].tcor.ToString("0.000");
                                    texte += ";" + links[arrivee].ncorr.ToString("0");
                                    texte += ";" + links[arrivee].tatt1.ToString("0.000");
                                    texte += ";" + links[arrivee].cout.ToString("0.000");
                                    texte += ";" + links[arrivee].l.ToString("0.000");
                                    texte += ";" + links[arrivee].pole;
                                    texte += ";" + od.ToString("0.00");
                                    //                                texte += ";" + links[arrivee].texte;
                                    //itineraire = "MAP," + itineraire;

                                    texte += ";" + itineraire;
                                    texte += ";" + projet.param_affectation_horaire.nb_pop;
                                    texte += ";" + links[arrivee].ttoll.ToString("0.000");


                                    fich_od.WriteLine(texte);

                                    if (projet.param_affectation_horaire.sortie_noeuds == true)
                                    {
                                        foreach (node n in nodes)
                                        {
                                            float tmax = 1e38f;
                                            int which_tmax = -1;
                                            String type_arc = "";
                                            link troncon = null;
                                            for (int s = 0; s < n.PredAll.Count; s++)
                                            {
                                                troncon = links[n.PredAll[s]];
                                                if (troncon.cout <= tmax && troncon.touche != 0 && (troncon.ligne <= 0 || projet.param_affectation_horaire.sortie_temps == 2))
                                                {
                                                    tmax = troncon.cout;
                                                    which_tmax = n.PredAll[s];
                                                    type_arc = troncon.type;
                                                }

                                            }
                                            if (which_tmax >= 0 && tmax <= projet.param_affectation_horaire.temps_max)
                                            {
                                                if (filtre.Contains(type_arc) || filtre.Count == 0)
                                                {
                                                    if (projet.param_affectation_horaire.sortie_temps == 3)
                                                    {
                                                        texte = p;
                                                        texte += ";" + n.i;
                                                        texte += ";" + (-horaire + links[which_tmax].h).ToString("0.000");
                                                        texte += ";" + links[which_tmax].tatt1.ToString("0.000");
                                                        texte += ";" + od.ToString("0.000");

                                                    }
                                                    else
                                                    {
                                                        texte = libod + ";" + p + ";" + q;
                                                        texte += ";" + jour.ToString("0.000");
                                                        texte += ";" + n.i;
                                                        texte += ";" + horaire.ToString("0.000");
                                                        texte += ";" + links[which_tmax].h.ToString("0.000");
                                                        texte += ";" + (-horaire + links[which_tmax].h).ToString("0.000");
                                                        texte += ";" + links[which_tmax].tveh.ToString("0.000");
                                                        texte += ";" + links[which_tmax].tmap.ToString("0.000");
                                                        texte += ";" + links[which_tmax].tatt.ToString("0.000");
                                                        texte += ";" + links[which_tmax].tcor.ToString("0.000");
                                                        texte += ";" + links[which_tmax].ncorr.ToString("0");
                                                        texte += ";" + links[which_tmax].tatt1.ToString("0.000");
                                                        texte += ";" + links[which_tmax].cout.ToString("0.000");
                                                        texte += ";" + links[which_tmax].l.ToString("0.000");
                                                        texte += ";" + links[which_tmax].pole;
                                                        texte += ";" + links[which_tmax].ttoll.ToString("0.000");
                                                        texte += ";" + od.ToString("0.000");
                                                        if (projet.param_affectation_horaire.sortie_stops == true)
                                                        {
                                                            texte += ";" + links[which_tmax].poleV2;
                                                        }
                                                        else
                                                        {
                                                            texte += ";";
                                                        }
                                                    }
                                                    fich_noeuds.WriteLine(texte);
                                                }
                                            }
                                        }
                                    }

                                    if (projet.param_affectation_horaire.sortie_temps == 0)
                                    {
                                        network reseau = projet.reseaux[projet.reseau_actif];
                                        double som_detour = 0; double nb_detour = 0; double som_oiseau = 0;
                                        double d_oiseau, d_link;
                                        foreach (link li in reseau.links)
                                        {
                                            d_oiseau = Math.Pow(Math.Pow((reseau.nodes[reseau.numnoeud[p]].x - reseau.nodes[li.nd].x), 2) + Math.Pow((reseau.nodes[reseau.numnoeud[p]].y - reseau.nodes[li.nd].y), 2), 0.5);
                                            d_link = Math.Pow(Math.Pow((reseau.nodes[li.no].x - reseau.nodes[li.nd].x), 2) + Math.Pow((reseau.nodes[li.no].y - reseau.nodes[li.nd].y), 2), 0.5);

                                            if (d_oiseau > 500 & d_oiseau - d_link <= 500)
                                            {

                                                if (reseau.nodes[reseau.numnoeud[p]].x > 0 && reseau.nodes[reseau.numnoeud[p]].y > 0 && reseau.nodes[li.nd].x > 0 && reseau.nodes[li.nd].y > 0)
                                                {
                                                    if (li.h > 0)
                                                    {
                                                        som_detour += li.h;
                                                        som_oiseau += Math.Pow(Math.Pow((reseau.nodes[reseau.numnoeud[p]].x - reseau.nodes[li.nd].x), 2) + Math.Pow((reseau.nodes[reseau.numnoeud[p]].y - reseau.nodes[li.nd].y), 2), 0.5);
                                                        nb_detour++;
                                                    }
                                                }
                                            }
                                        }
                                        fich_detour.WriteLine(p + ";" + som_detour.ToString() + ";" + som_oiseau.ToString() + ";" + nb_detour.ToString());

                                    }

                                    if (projet.param_affectation_horaire.sortie_temps > 0 && projet.param_affectation_horaire.sortie_temps < 4)
                                    {
                                        for (i = 0; i < links.Count; i++)
                                        {
                                            if (links[i].orig >= 0) continue; arrivee = MeilleurEtat(links, i, w.cloF);


                                            if (filtre.Contains(links[arrivee].type) || filtre.Count == 0)
                                            {
                                                if (links[arrivee].touche != 0 && (links[arrivee].ligne < 0 || projet.param_affectation_horaire.sortie_temps == 2))
                                                {
                                                    if (projet.param_affectation_horaire.sortie_noeuds == false && projet.param_affectation_horaire.sortie_temps == 3)
                                                    {
                                                        texte = p;
                                                        texte += ";" + nodes[links[arrivee].no].i;
                                                        texte += "-" + nodes[links[arrivee].nd].i;
                                                        texte += ";" + (-horaire + links[arrivee].h).ToString("0.000");
                                                        texte += ";" + links[arrivee].tatt1.ToString("0.000");
                                                        texte += ";" + od.ToString("0.000");


                                                    }
                                                    else if (projet.param_affectation_horaire.sortie_temps < 3)
                                                    {
                                                        texte = libod + ";" + p;
                                                        texte += ";" + nodes[links[arrivee].no].i;
                                                        texte += "-" + nodes[links[arrivee].nd].i;
                                                        texte += ";" + (links[arrivee].ligne).ToString("0");
                                                        texte += ";" + i.ToString("0");
                                                        texte += ";" + jour.ToString("0");
                                                        texte += ";" + horaire.ToString("0.000");
                                                        texte += ";" + links[arrivee].h.ToString("0.000");
                                                        texte += ";" + (-horaire + links[arrivee].h).ToString("0.000");
                                                        texte += ";" + links[arrivee].tveh.ToString("0.000");
                                                        texte += ";" + links[arrivee].tmap.ToString("0.000");
                                                        texte += ";" + links[arrivee].tatt.ToString("0.000");
                                                        texte += ";" + links[arrivee].tcor.ToString("0.000");
                                                        texte += ";" + links[arrivee].ncorr.ToString("0");
                                                        texte += ";" + links[arrivee].tatt1.ToString("0.000");
                                                        texte += ";" + links[arrivee].cout.ToString("0.000");
                                                        texte += ";" + links[arrivee].l.ToString("0.000");
                                                        texte += ";" + links[arrivee].pole;
                                                        texte += ";" + od.ToString("0.00");
                                                        // texte += ";" + links[arrivee].texte;
                                                        /*texte += ";" + projet.param_affectation_horaire.texte_cveh;
                                                        texte += ";" + projet.param_affectation_horaire.texte_cwait;
                                                        texte += ";" + projet.param_affectation_horaire.texte_cmap;
                                                        texte += ";" + projet.param_affectation_horaire.texte_cboa;
                                                        texte += ";" + projet.param_affectation_horaire.texte_coef_tmap;
                                                        texte += ";" + projet.param_affectation_horaire.texte_tboa;
                                                        texte += ";" + projet.param_affectation_horaire.nb_jours;*/
                                                        texte += ";" + links[arrivee].pivot.ToString("0");
                                                        texte += ";" + links[arrivee].type;
                                                        texte += ";" + links[arrivee].ttoll.ToString("0.000");

                                                        link arc = null;
                                                        arc = links[arrivee];
                                                        float ti;
                                                        if (arc.ligne < 0)
                                                        {
                                                            ti = -horaire + (arc.h - arc.temps * projet.param_affectation_horaire.fcoef_tmap[arc.type]);

                                                            //ti = -horaire + arc.h - arc.temps;
                                                        }
                                                        else
                                                        {
                                                            ti = -horaire + arc.h - (arc.services[arc.service].hf - arc.services[arc.service].hd);
                                                        }
                                                        texte += ";" + ti.ToString("0.000");

                                                    }
                                                    //                                itineraire = "MAP," + itineraire;
                                                    //texte += ";" + itineraire;
                                                    if ((links[arrivee].cout) <= projet.param_affectation_horaire.temps_max)
                                                    {

                                                        if (test_temps_per_min_depart(projet, arrivee) == true)
                                                        {
                                                            if (!(projet.param_affectation_horaire.sortie_temps == 3 && projet.param_affectation_horaire.sortie_noeuds == true))
                                                            {
                                                                fich_sortie.WriteLine(texte);
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (links[arrivee].touche == 0 && projet.param_affectation_horaire.sortie_isoles == true)
                                                {
                                                    texte = nodes[links[arrivee].no].i;
                                                    texte += "-" + nodes[links[arrivee].nd].i;
                                                    texte += ";" + (links[arrivee].ligne).ToString("0");
                                                    texte += ";" + i.ToString("0");
                                                    fich_isoles.WriteLine(texte);

                                                }

                                            }
                                        }
                                    }
                                }
                            }

                            // sens heure d'arrivée
                            // sens heure d'arrivée
                            // sens heure d'arrivée
                            // sens heure d'arrivée
                            // sens heure d'arrivée
                            // sens heure d'arrivée
                            // sens heure d'arrivée
                            if (sens == 2)
                            {
                                // (test de doublon deplace dans la lecture sequentielle de la matrice : voir task.skip)
                                if (skip) goto fin_gga2;

                                for (i = 0; i < links.Count; i++)
                                {
                                    links[i].touche = 0;
                                    links[i].cout = 0;
                                    links[i].tatt = 0;
                                    links[i].tatt1 = 0;
                                    links[i].tcor = 0;
                                    links[i].ncorr = 0;
                                    links[i].tmap = 0;
                                    links[i].tveh = 0;
                                    links[i].h = 0;
                                    links[i].ttoll = 0;
                                    links[i].l = 0;
                                    for (j = 0; j < links[i].services.Count; j++)
                                    {
                                        links[i].services[j].delta = 0;
                                        links[i].services[j].boai = 0;
                                        links[i].services[j].alij = 0;
                                    }
                                    links[i].pivot = -1;
                                    links[i].turn_pivot = -1;
                                    links[i].pole = "-1";
                                    links[i].poleV2 = "";
                                    links[i].service = -1;
                                    links[i].is_queue = false;



                                }
                                gga_nq.Clear();
                                string depart = q;
                                int pivot = -1, value;
                                int bucket, id_bucket = 0, predecesseur;
                                float penalite = 0, temps_correspondance, max_correspondance;
                                if (projet.reseaux[projet.reseau_actif].numnoeud.TryGetValue(depart, out value) == true)
                                {

                                    for (j = 0; j < nodes[projet.reseaux[projet.reseau_actif].numnoeud[depart]].pred.Count; j++)
                                    {
                                        predecesseur = nodes[projet.reseaux[projet.reseau_actif].numnoeud[depart]].pred[j];
                                        String pred_type = links[predecesseur].type;
                                        max_correspondance = projet.param_affectation_horaire.ftboa_max[pred_type];




                                        if (links[predecesseur].ligne < 0 && projet.param_affectation_horaire.fcmap[pred_type] > 0 && links[predecesseur].temps < projet.param_affectation_horaire.tmapmax)
                                        {
                                            bool test_periode = false;

                                            if (links[predecesseur].services.Count > 0)
                                            {
                                                int decal_jour = (int)Math.Floor(horaire / 1440f);
                                                for (int kk = 0; kk < links[predecesseur].services.Count; kk++)
                                                {
                                                    if (Math.Abs(decal_jour) <= projet.param_affectation_horaire.nb_jours)
                                                    {
                                                        if (projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[kk].regime][jour + decal_jour] == 'O' && links[predecesseur].services[kk].hd + 1440f * decal_jour <= horaire && links[predecesseur].services[kk].hf + 1440f * decal_jour > horaire)
                                                        {
                                                            test_periode = true;
                                                            links[predecesseur].service = kk;
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                test_periode = true;
                                            }

                                            if (test_periode == true)
                                            {


                                                //touches.Enqueue(successeur);
                                                links[predecesseur].touche = 1;
                                                links[predecesseur].cout = (links[predecesseur].temps) * projet.param_affectation_horaire.fcoef_tmap[pred_type] * projet.param_affectation_horaire.fcmap[pred_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type];
                                                links[predecesseur].tmap = (links[predecesseur].temps) * projet.param_affectation_horaire.fcoef_tmap[pred_type];
                                                links[predecesseur].ttoll = links[predecesseur].toll;

                                                links[predecesseur].h = horaire - (links[predecesseur].temps) * projet.param_affectation_horaire.fcoef_tmap[pred_type];
                                                links[predecesseur].l = links[predecesseur].longueur;
                                                links[predecesseur].pivot = -1;
                                                links[predecesseur].turn_pivot = -1;

                                                links[predecesseur].pole = depart;
                                                links[predecesseur].poleV2 = "";
                                                //                                    bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[predecesseur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                while (bucket >= gga_nq.Count)
                                                {
                                                    gga_nq.Add(new List<int>());
                                                }
                                                gga_nq[bucket].Add(predecesseur);
                                                projet.param_affectation_horaire.nb_pop++;
                                            }
                                        }
                                        else if (projet.param_affectation_horaire.fcveh[pred_type] > 0)
                                        {
                                            int ii, jj, num_service = -1, h3 = 0, delta, duree_periode;
                                            float h1 = 1e38f, h2 = 1e38f, cout2 = 1e38f;
                                            for (ii = 0; ii < links[predecesseur].services.Count; ii++)
                                            {
                                                delta = 0;
                                                duree_periode = projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[ii].regime].Length;

                                                if ((links[predecesseur].services[ii].hf > horaire) || projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[ii].regime][jour] == 'N')
                                                {

                                                    h1 = -1e38f;
                                                    h2 = 1e38f;
                                                    h3 = -1;
                                                    for (jj = jour - 1; jj >= Math.Max(jour - projet.param_affectation_horaire.nb_jours, 0); jj--)
                                                    {
                                                        if (projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[ii].regime][jj] == 'O' && (links[predecesseur].services[ii].hf + (-jour + jj) * 24f * 60f) > h1 && (links[predecesseur].services[ii].hf + (-jour + jj) * 24f * 60f) < horaire)
                                                        {
                                                            h1 = links[predecesseur].services[ii].hf + (-jour + jj) * 24f * 60f;
                                                            h2 = (-jour + jj);
                                                            h3 = jj;
                                                        }

                                                    }
                                                    if (h3 != -1)
                                                    {
                                                        links[predecesseur].services[ii].delta = h2;
                                                    }
                                                    else
                                                    {
                                                        delta = 1;
                                                    }


                                                }

                                                if (-links[predecesseur].services[ii].hf - links[predecesseur].services[ii].delta * 60f * 24f + horaire < max_correspondance)
                                                {
                                                    if (((links[predecesseur].services[ii].hf - links[predecesseur].services[ii].hd) * projet.param_affectation_horaire.fcveh[pred_type] + (-links[predecesseur].services[ii].hf - links[predecesseur].services[ii].delta * 60f * 24f + horaire) * projet.param_affectation_horaire.fcwait[pred_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type]) < cout2 && delta < 1)
                                                    {
                                                        cout2 = (links[predecesseur].services[ii].hf - links[predecesseur].services[ii].hd) * projet.param_affectation_horaire.fcveh[pred_type] + (-links[predecesseur].services[ii].hf - links[predecesseur].services[ii].delta * 60f * 24f + horaire) * projet.param_affectation_horaire.fcwait[pred_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type];
                                                        num_service = ii;

                                                    }
                                                }

                                            }
                                            bool av_set = false, av_alt = false; float av_cout = 0f, av_delta = 0f; int av_idx = -1;
                                                    if (av_on)
                                                    {
                                                        AvArrivee(projet, links[predecesseur], jour, horaire, horaire, max_correspondance, 0f, projet.param_affectation_horaire.fcveh[pred_type], projet.param_affectation_horaire.fcwait[pred_type], links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type], pa, pr, num_service, out av_alt, out av_idx, out av_delta, out av_cout);
                                                        av_set = (av_cout < 1e37f);
                                                        if (av_alt) { num_service = av_idx; links[predecesseur].services[av_idx].delta = av_delta; }
                                                    }
                                                    if (num_service != -1)
                                            {
                                                links[predecesseur].service = num_service;
                                                links[predecesseur].cout = (links[predecesseur].services[num_service].hf - links[predecesseur].services[num_service].hd) * projet.param_affectation_horaire.fcveh[pred_type] + (-links[predecesseur].services[num_service].hf - links[predecesseur].services[num_service].delta * 1440f + horaire) * projet.param_affectation_horaire.fcwait[pred_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type];

                                                links[predecesseur].touche = 1;

                                                links[predecesseur].h = links[predecesseur].services[links[predecesseur].service].hd + links[predecesseur].services[links[predecesseur].service].delta * 60f * 24f;
                                                links[predecesseur].tatt = -links[predecesseur].services[links[predecesseur].service].hf - links[predecesseur].services[links[predecesseur].service].delta * 1440f + horaire;
                                                links[predecesseur].tatt1 = -links[predecesseur].services[links[predecesseur].service].hf - links[predecesseur].services[links[predecesseur].service].delta * 1440f + horaire;
                                                links[predecesseur].tveh = links[predecesseur].services[links[predecesseur].service].hf - links[predecesseur].services[links[predecesseur].service].hd;
                                                links[predecesseur].tcor = 0;
                                                links[predecesseur].ncorr = 1;
                                                links[predecesseur].l = links[predecesseur].longueur;
                                                links[predecesseur].tmap = 0;
                                                links[predecesseur].ttoll = links[predecesseur].toll;
                                                        if (av_set) { links[predecesseur].cout = av_cout; }

                                                //bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[predecesseur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                while (bucket >= gga_nq.Count)
                                                {
                                                    gga_nq.Add(new List<int>());
                                                }
                                                gga_nq[bucket].Add(predecesseur);
                                                projet.param_affectation_horaire.nb_pop++;
                                                //                                touches.Enqueue(successeur);
                                                links[predecesseur].pivot = -1;
                                                links[predecesseur].turn_pivot = -1;
                                                links[predecesseur].pole = nodes[links[predecesseur].nd].i;
                                                links[predecesseur].poleV2 = nodes[links[predecesseur].nd].i;
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    fich_log.WriteLine("OD error" + libod + ":" + chaine + ": non existing destination node!");
                                }
                                int bucket_cout_max = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(projet.param_affectation_horaire.temps_max / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));

                                while (gga_nq.Count > id_bucket && bucket_cout_max >= id_bucket)
                                {

                                    while (gga_nq[id_bucket].Count == 0)
                                    {
                                        id_bucket++;
                                        if (id_bucket >= gga_nq.Count || bucket_cout_max == id_bucket+1)
                                        {
                                            goto fin_gga2;
                                        }
                                    }
                                    if (projet.param_affectation_horaire.algorithme == 0)
                                    {
                                        pivot = gga_nq[id_bucket][0];
                                        gga_nq[id_bucket].RemoveAt(0);

                                    }
                                    else
                                    {
                                        int k, id_pivot = -1; double cout_max = 1e38f;
                                        for (k = 0; k < gga_nq[id_bucket].Count; k++)
                                        {
                                            if (links[gga_nq[id_bucket][k]].cout < cout_max)
                                            {
                                                cout_max = links[gga_nq[id_bucket][k]].cout;
                                                id_pivot = k;
                                            }
                                        }
                                        pivot = gga_nq[id_bucket][id_pivot];
                                        gga_nq[id_bucket].RemoveAt(id_pivot);
                                        links[pivot].touche = 3;
                                    }


                                    //avancement.textBox1.Text = touches.Count.ToString() + " " + calcules.Count.ToString() + " " + links[pivot].cout;
                                    //avancement.textBox1.Refresh();
                                    for (j = 0; j < nodes[links[pivot].no].pred.Count; j++)
                                    {

                                        String pivot_type = links[pivot].type;
                                        link troncon_pred = links[nodes[links[pivot].no].pred[j]];
                                        link troncon_pivot = links[pivot];
                                        predecesseur = nodes[links[pivot].no].pred[j];
                                if (w.redirB.Count > 0) { int rt; if (w.redirB.TryGetValue(((long)pivot << 32) | (uint)predecesseur, out rt)) { predecesseur = rt; troncon_pred = links[predecesseur]; } }
                                        String pred_type = links[predecesseur].type;

                                        if (projet.param_affectation_horaire.demitours == true)
                                        {

                                            if (troncon_pivot.nd == troncon_pred.no)
                                            {

                                                penalite = -1;
                                            }
                                            else
                                            {
                                                penalite = 0;
                                            }
                                        }

                                        else
                                        {
                                            penalite = 0;
                                        }
                                        Turn virage = w.turnTest;
                                        virage.arci = predecesseur;
                                        virage.arcj = pivot;
                                        float value2;
                                        if (nodes[troncon_pivot.no].is_intersection == true)
                                        {
                                            if (turns.TryGetValue(virage, out value2) == true)
                                            {
                                                penalite = turns[virage];
                                                pred_type = links[predecesseur].type;
                                            }
                                            else
                                            {
                                                penalite = 0;
                                            }
                                        }



                                        float tpen = 0f, tpc = 0f;   // peage du virage (fichier de penalites) et son cout
                                        if (penalite >= 0 && w.turnsToll.Count > 0 && nodes[troncon_pivot.no].is_intersection == true && w.turnsToll.TryGetValue(virage, out tpen)) tpc = tpen * projet.param_affectation_horaire.fctoll[pivot_type];
                                        if (penalite >= 0 && w.turnsCost.Count > 0 && nodes[troncon_pivot.no].is_intersection == true) { float tcv; if (w.turnsCost.TryGetValue(virage, out tcv)) tpc += tcv; }
                                        if (penalite >= 0)
                                        {
                                            if (penalite > 0)
                                            {
                                                temps_correspondance = penalite + projet.param_affectation_horaire.ftboa[pivot_type];
                                                max_correspondance = projet.param_affectation_horaire.ftboa_max[pivot_type];

                                            }
                                            else
                                            {
                                                temps_correspondance = projet.param_affectation_horaire.ftboa[pivot_type];
                                                max_correspondance = projet.param_affectation_horaire.ftboa_max[pivot_type];

                                            }
                                            //successeurs touches pour la première fois
                                            if (links[predecesseur].touche == 0)
                                            {
                                                // predecesseur marche à pied pivot marche
                                                if (links[predecesseur].ligne < 0 && links[pivot].ligne < 0 && projet.param_affectation_horaire.fcmap[pred_type] > 0 && (links[pivot].tmap + links[predecesseur].temps < projet.param_affectation_horaire.tmapmax))
                                                {
                                                    bool test_periode = false;
                                                    links[predecesseur].service = -1;
                                                    if (links[predecesseur].services.Count > 0)
                                                    {
                                                        int decal_jour = (int)(Math.Floor((links[pivot].h - penalite) / 1440f));
                                                        for (int kk = 0; kk < links[predecesseur].services.Count; kk++)
                                                        {
                                                            if (Math.Abs(decal_jour) <= projet.param_affectation_horaire.nb_jours)
                                                            {
                                                                if (projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[kk].regime][jour + decal_jour] == 'O' && links[predecesseur].services[kk].hd + 1440f * decal_jour <= links[pivot].h - penalite && links[predecesseur].services[kk].hf + 1440f * decal_jour > links[pivot].h - penalite)
                                                                {
                                                                    test_periode = true;
                                                                    links[predecesseur].service = kk;
                                                                }
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        test_periode = true;
                                                    }

                                                    if (test_periode == true)
                                                    {

                                                        links[predecesseur].cout = links[pivot].cout + (links[predecesseur].temps + penalite) * projet.param_affectation_horaire.fcoef_tmap[pred_type] * projet.param_affectation_horaire.fcmap[pred_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type];
                                                        links[predecesseur].h = links[pivot].h - (links[predecesseur].temps) * projet.param_affectation_horaire.fcoef_tmap[pred_type] - penalite;
                                                        links[predecesseur].tatt = links[pivot].tatt;
                                                        links[predecesseur].tatt1 = links[pivot].tatt1;
                                                        links[predecesseur].tveh = links[pivot].tveh;
                                                        links[predecesseur].tcor = links[pivot].tcor;
                                                        links[predecesseur].ncorr = links[pivot].ncorr;
                                                        links[predecesseur].tmap = links[pivot].tmap + (links[predecesseur].temps + penalite) * projet.param_affectation_horaire.fcoef_tmap[pred_type];
                                                        links[predecesseur].ttoll = links[pivot].ttoll + links[predecesseur].toll;
                                                    if (tpen != 0f || tpc != 0f) { links[predecesseur].ttoll += tpen; links[predecesseur].cout += tpc; if (tpc < 0f && links[predecesseur].cout < links[pivot].cout) links[predecesseur].cout = links[pivot].cout; }

                                                        links[predecesseur].l = links[pivot].l + links[predecesseur].longueur;
                                                        links[predecesseur].touche = 1;

                                                        //bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[predecesseur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                        bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                        while (bucket >= gga_nq.Count)
                                                        {
                                                            gga_nq.Add(new List<int>());
                                                        }
                                                        gga_nq[bucket].Add(predecesseur);
                                                        projet.param_affectation_horaire.nb_pop++;
                                                        //                                        touches.Enqueue(successeur);
                                                        links[predecesseur].pivot = pivot;
                                                        links[predecesseur].turn_pivot = j;
                                                        links[predecesseur].pole = links[pivot].pole;
                                                        links[predecesseur].poleV2 = links[pivot].poleV2;
                                                    }
                                                }
                                                // predecesseur marche à pied pivot TC
                                                else if (links[predecesseur].ligne < 0 && links[pivot].ligne > 0 && projet.param_affectation_horaire.fcmap[pred_type] > 0 && (links[pivot].tmap + links[predecesseur].temps < projet.param_affectation_horaire.tmapmax))
                                                {
                                                    bool test_periode = false;
                                                    links[predecesseur].service = -1;


                                                    if (links[predecesseur].services.Count > 0)
                                                    {
                                                        int decal_jour = -(int)(Math.Floor((links[pivot].h - temps_correspondance) / 1440f));
                                                        for (int kk = 0; kk < links[predecesseur].services.Count; kk++)
                                                        {
                                                            if (decal_jour <= projet.param_affectation_horaire.nb_jours)
                                                            {
                                                                if (projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[kk].regime][jour - decal_jour] == 'O' && links[predecesseur].services[kk].hd - 1440f * decal_jour <= links[pivot].h - temps_correspondance && links[predecesseur].services[kk].hf - 1440f * decal_jour > links[pivot].h - temps_correspondance)
                                                                {
                                                                    test_periode = true;
                                                                    links[predecesseur].service = kk;
                                                                }
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        test_periode = true;
                                                    }

                                                    if (test_periode == true)
                                                    {


                                                        links[predecesseur].cout = links[pivot].cout + (links[predecesseur].temps + penalite) * projet.param_affectation_horaire.fcoef_tmap[pred_type] * projet.param_affectation_horaire.fcmap[pred_type] + projet.param_affectation_horaire.fcboa[pivot_type] * temps_correspondance + temps_correspondance * projet.param_affectation_horaire.fcwait[pred_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type];
                                                        links[predecesseur].h = links[pivot].h - (links[predecesseur].temps) * projet.param_affectation_horaire.fcoef_tmap[pred_type] - temps_correspondance - penalite;
                                                        links[predecesseur].tatt = links[pivot].tatt + temps_correspondance;
                                                        links[predecesseur].tatt1 = links[pivot].tatt1;
                                                        links[predecesseur].tveh = links[pivot].tveh;
                                                        links[predecesseur].tcor = links[pivot].tcor + temps_correspondance;
                                                        links[predecesseur].ncorr = links[pivot].ncorr + 1;
                                                        links[predecesseur].tmap = links[pivot].tmap + (links[predecesseur].temps + penalite) * projet.param_affectation_horaire.fcoef_tmap[pred_type];
                                                        links[predecesseur].ttoll = links[pivot].ttoll + links[predecesseur].toll;
                                                    if (tpen != 0f || tpc != 0f) { links[predecesseur].ttoll += tpen; links[predecesseur].cout += tpc; if (tpc < 0f && links[predecesseur].cout < links[pivot].cout) links[predecesseur].cout = links[pivot].cout; }

                                                        links[predecesseur].l = links[pivot].l + links[predecesseur].longueur;
                                                        links[predecesseur].touche = 1;
                                                        // bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[predecesseur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                        bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                        while (bucket >= gga_nq.Count)
                                                        {
                                                            gga_nq.Add(new List<int>());
                                                        }
                                                        gga_nq[bucket].Add(predecesseur);
                                                        projet.param_affectation_horaire.nb_pop++;
                                                        //                                        touches.Enqueue(successeur);
                                                        links[predecesseur].pivot = pivot;
                                                        links[predecesseur].turn_pivot = j;
                                                        links[predecesseur].pole = nodes[links[predecesseur].nd].i;
                                                        links[predecesseur].poleV2 = "|" + nodes[links[predecesseur].nd].i + links[pivot].poleV2;
                                                    }
                                                }
                                                //predecesseurs TC même ligne
                                                else if (links[predecesseur].ligne == links[pivot].ligne && links[predecesseur].ligne > 0 && projet.param_affectation_horaire.fcveh[pred_type] > 0)
                                                {
                                                    int ii, num_service = -1;
                                                    for (ii = 0; ii < links[predecesseur].services.Count; ii++)
                                                    {
                                                        if (links[predecesseur].services[ii].numero == links[pivot].services[links[pivot].service].numero)
                                                        {
                                                            if (links[predecesseur].services[ii].hf <= links[pivot].services[links[pivot].service].hd)
                                                            {
                                                                num_service = ii;
                                                            }
                                                        }
                                                    }
                                                    if (num_service != -1 && links[predecesseur].services[num_service].hf + links[pivot].services[links[pivot].service].delta * 1440f <= links[pivot].h)
                                                    {
                                                        links[predecesseur].service = num_service;
                                                        links[predecesseur].services[num_service].delta = links[pivot].services[links[pivot].service].delta;

                                                        links[predecesseur].touche = 1;
                                                        links[predecesseur].cout = links[pivot].cout + (-links[predecesseur].services[links[predecesseur].service].hd - links[predecesseur].services[links[predecesseur].service].delta * 1440f + links[pivot].h) * projet.param_affectation_horaire.fcveh[pred_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type];
                                                        links[predecesseur].h = links[predecesseur].services[links[predecesseur].service].hd + links[predecesseur].services[links[predecesseur].service].delta * 60f * 24f;
                                                        links[predecesseur].tatt = links[pivot].tatt;
                                                        links[predecesseur].tatt1 = links[pivot].tatt1;
                                                        links[predecesseur].tveh = links[pivot].tveh - links[predecesseur].services[links[predecesseur].service].hd - links[predecesseur].services[links[predecesseur].service].delta * 1440f + links[pivot].h;
                                                        links[predecesseur].tcor = links[pivot].tcor;
                                                        links[predecesseur].ncorr = links[pivot].ncorr;
                                                        links[predecesseur].l = links[pivot].l + links[predecesseur].longueur;
                                                        links[predecesseur].tmap = links[pivot].tmap;
                                                        links[predecesseur].ttoll = links[pivot].ttoll + links[predecesseur].toll;
                                                    if (tpen != 0f || tpc != 0f) { links[predecesseur].ttoll += tpen; links[predecesseur].cout += tpc; if (tpc < 0f && links[predecesseur].cout < links[pivot].cout) links[predecesseur].cout = links[pivot].cout; }

                                                        //bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[predecesseur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                        bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                        while (bucket >= gga_nq.Count)
                                                        {
                                                            gga_nq.Add(new List<int>());
                                                        }
                                                        gga_nq[bucket].Add(predecesseur);
                                                        projet.param_affectation_horaire.nb_pop++;
                                                        //touches.Enqueue(successeur);
                                                        links[predecesseur].pivot = pivot;
                                                        links[predecesseur].turn_pivot = j;
                                                        links[predecesseur].pole = links[pivot].pole;
                                                        links[predecesseur].poleV2 = links[pivot].poleV2;
                                                    }
                                                }

                                                //predecesseur TC lignes différentes
                                                else if (links[predecesseur].ligne != links[pivot].ligne && links[pivot].ligne > 0 && links[predecesseur].ligne > 0 && projet.param_affectation_horaire.fcveh[pred_type] > 0)
                                                {
                                                    int ii, jj, num_service = -1, h3 = 0, delta, duree_periode;
                                                    float h1 = -1e38f, h2 = 1e38f, cout2 = 1e38f;
                                                    for (ii = 0; ii < links[predecesseur].services.Count; ii++)
                                                    {
                                                        delta = 0;
                                                        duree_periode = projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[ii].regime].Length;

                                                        if ((links[predecesseur].services[ii].hf + temps_correspondance > links[pivot].h) || projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[ii].regime][jour] == 'N')
                                                        {

                                                            h1 = -1e38f;
                                                            h2 = 1e38f;
                                                            h3 = -1;
                                                            for (jj = jour - 1; jj >= Math.Max(jour - projet.param_affectation_horaire.nb_jours, 0); jj--)
                                                            {
                                                                if (projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[ii].regime][jj] == 'O' && (links[predecesseur].services[ii].hf + (-jour + jj) * 24f * 60f) > h1 && (links[predecesseur].services[ii].hf + (-jour + jj) * 24f * 60f) + temps_correspondance < links[pivot].h)
                                                                {
                                                                    h1 = links[predecesseur].services[ii].hf + (-jour + jj) * 24f * 60f;
                                                                    h2 = (-jour + jj);
                                                                    h3 = jj;
                                                                }

                                                            }
                                                            if (h3 != -1)
                                                            {
                                                                if (links[predecesseur].services[ii].delta > h2 || links[predecesseur].touche == 0)
                                                                {
                                                                    links[predecesseur].services[ii].delta = h2;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                delta = 1;
                                                            }


                                                        }
                                                        if ((links[predecesseur].services[ii].hf + links[predecesseur].services[ii].delta * 1440f + max_correspondance > links[pivot].h) && (links[predecesseur].services[ii].hf + links[predecesseur].services[ii].delta * 1440f + temps_correspondance <= links[pivot].h))
                                                        {
                                                            if ((links[pivot].cout + (links[predecesseur].services[ii].hf - links[predecesseur].services[ii].hd) * projet.param_affectation_horaire.fcveh[pred_type] + (-links[predecesseur].services[ii].hf - links[predecesseur].services[ii].delta * 60f * 24f + links[pivot].h) * projet.param_affectation_horaire.fcwait[pred_type] + temps_correspondance * projet.param_affectation_horaire.fcboa[pivot_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type]) < cout2 && delta < 1)
                                                            {

                                                                cout2 = links[pivot].cout + (links[predecesseur].services[ii].hf - links[predecesseur].services[ii].hd) * projet.param_affectation_horaire.fcveh[pred_type] + (-links[predecesseur].services[ii].hf - links[predecesseur].services[ii].delta * 60f * 24f + links[pivot].h) * projet.param_affectation_horaire.fcwait[pred_type] + temps_correspondance * projet.param_affectation_horaire.fcboa[pivot_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type];
                                                                num_service = ii;

                                                            }
                                                        }

                                                    }
                                                    if (num_service != -1)
                                                    {
                                                        links[predecesseur].service = num_service;
                                                        links[predecesseur].cout = links[pivot].cout + (links[predecesseur].services[num_service].hf - links[predecesseur].services[num_service].hd) * projet.param_affectation_horaire.fcveh[pred_type] + (-links[predecesseur].services[num_service].hf - links[predecesseur].services[num_service].delta * 1440f + links[pivot].h) * projet.param_affectation_horaire.fcwait[pivot_type] + (temps_correspondance * projet.param_affectation_horaire.fcboa[pivot_type]) + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type];

                                                        links[predecesseur].touche = 1;

                                                        links[predecesseur].h = links[predecesseur].services[links[predecesseur].service].hd + links[predecesseur].services[links[predecesseur].service].delta * 60f * 24f;
                                                        if (links[pivot].ncorr == 0)
                                                        {
                                                            links[predecesseur].tatt1 = -links[predecesseur].services[links[predecesseur].service].hf - links[predecesseur].services[links[predecesseur].service].delta * 1440f + links[pivot].h;
                                                        }
                                                        else
                                                        {
                                                            links[predecesseur].tatt1 = links[pivot].tatt1;
                                                        }

                                                        links[predecesseur].tatt = links[pivot].tatt - links[predecesseur].services[links[predecesseur].service].hf - links[predecesseur].services[links[predecesseur].service].delta * 1440f + links[pivot].h;
                                                        links[predecesseur].tveh = links[pivot].tveh + links[predecesseur].services[links[predecesseur].service].hf - links[predecesseur].services[links[predecesseur].service].hd;
                                                        links[predecesseur].tcor = links[pivot].tcor + temps_correspondance;
                                                        links[predecesseur].ncorr = links[pivot].ncorr + 1;
                                                        links[predecesseur].l = links[pivot].l + links[predecesseur].longueur;
                                                        links[predecesseur].tmap = links[pivot].tmap;
                                                        links[predecesseur].ttoll = links[pivot].ttoll + links[predecesseur].toll;
                                                    if (tpen != 0f || tpc != 0f) { links[predecesseur].ttoll += tpen; links[predecesseur].cout += tpc; if (tpc < 0f && links[predecesseur].cout < links[pivot].cout) links[predecesseur].cout = links[pivot].cout; }

                                                        //bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[predecesseur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                        bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                        while (bucket >= gga_nq.Count)
                                                        {
                                                            gga_nq.Add(new List<int>());
                                                        }
                                                        gga_nq[bucket].Add(predecesseur);
                                                        projet.param_affectation_horaire.nb_pop++;
                                                        //                                        touches.Enqueue(successeur);
                                                        links[predecesseur].pivot = pivot;
                                                        links[predecesseur].turn_pivot = j;
                                                        links[predecesseur].pole = nodes[links[predecesseur].nd].i;
                                                        links[predecesseur].poleV2 = "|" + nodes[links[predecesseur].nd].i + "|" + nodes[links[predecesseur].nd].i + links[pivot].poleV2;
                                                    }
                                                }

                                                //predecesseur TC lignes différentes pivot MAP
                                                else if (links[predecesseur].ligne > 0 && links[pivot].ligne < 0 && projet.param_affectation_horaire.fcveh[pred_type] > 0)
                                                {
                                                    int ii, jj, num_service = -1, h3 = 0, delta, duree_periode;
                                                    float h1 = 1e38f, h2 = 1e38f, cout2 = 1e38f;
                                                    for (ii = 0; ii < links[predecesseur].services.Count; ii++)
                                                    {
                                                        delta = 0;
                                                        duree_periode = projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[ii].regime].Length;
                                                        if ((links[predecesseur].services[ii].hf > links[pivot].h) || projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[ii].regime][jour] == 'N')
                                                        {
                                                            h1 = -1e38f;
                                                            h2 = 1e38f;
                                                            h3 = -1;

                                                            for (jj = jour - 1; jj >= Math.Max(jour - projet.param_affectation_horaire.nb_jours, 0); jj--)
                                                            {

                                                                if (projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[ii].regime][jj] == 'O' && (links[predecesseur].services[ii].hf + (-jour + jj) * 24f * 60f) > h1 && (links[predecesseur].services[ii].hf + (-jour + jj) * 24f * 60f) < links[pivot].h)
                                                                {
                                                                    h1 = links[predecesseur].services[ii].hf + (-jour + jj) * 24f * 60f;
                                                                    h2 = (-jour + jj);
                                                                    h3 = jj;
                                                                }

                                                            }
                                                            if (h3 != -1)
                                                            {
                                                                if (links[predecesseur].services[ii].delta > h2 || links[predecesseur].touche == 0)
                                                                {
                                                                    links[predecesseur].services[ii].delta = h2;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                delta = 1;
                                                            }


                                                        }
                                                        if ((links[predecesseur].services[ii].hf + links[predecesseur].services[ii].delta * 1440f <= links[pivot].h))

                                                        {
                                                            if ((links[pivot].cout + (links[predecesseur].services[ii].hf - links[predecesseur].services[ii].hd) * projet.param_affectation_horaire.fcveh[pred_type] + (-links[predecesseur].services[ii].hf - links[predecesseur].services[ii].delta * 60f * 24f + links[pivot].h) * projet.param_affectation_horaire.fcwait[pred_type]) + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type] < cout2 && delta < 1)
                                                            {
                                                                cout2 = links[pivot].cout + (links[predecesseur].services[ii].hf - links[predecesseur].services[ii].hd) * projet.param_affectation_horaire.fcveh[pred_type] + (-links[predecesseur].services[ii].hf - links[predecesseur].services[ii].delta * 60f * 24f + links[pivot].h) * projet.param_affectation_horaire.fcwait[pred_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type];
                                                                num_service = ii;

                                                            }
                                                        }

                                                    }
                                                    bool av_set = false, av_alt = false; float av_cout = 0f, av_delta = 0f; int av_idx = -1;
                                                    if (av_on && links[pivot].ncorr == 0)
                                                    {
                                                        AvArrivee(projet, links[predecesseur], jour, links[pivot].h, links[pivot].h, max_correspondance, links[pivot].cout, projet.param_affectation_horaire.fcveh[pred_type], projet.param_affectation_horaire.fcwait[pred_type], links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type], pa, pr, num_service, out av_alt, out av_idx, out av_delta, out av_cout);
                                                        av_set = (av_cout < 1e37f);
                                                        if (av_alt) { num_service = av_idx; links[predecesseur].services[av_idx].delta = av_delta; }
                                                    }
                                                    if (num_service != -1)
                                                    {
                                                        links[predecesseur].service = num_service;
                                                        links[predecesseur].cout = links[pivot].cout + (links[predecesseur].services[num_service].hf - links[predecesseur].services[num_service].hd) * projet.param_affectation_horaire.fcveh[pred_type] + (-links[predecesseur].services[num_service].hf - links[predecesseur].services[num_service].delta * 1440f + links[pivot].h) * projet.param_affectation_horaire.fcwait[pred_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type];

                                                        links[predecesseur].touche = 1;

                                                        links[predecesseur].h = links[predecesseur].services[links[predecesseur].service].hd + links[predecesseur].services[links[predecesseur].service].delta * 24f * 60f;
                                                        if (links[pivot].ncorr == 0)
                                                        {
                                                            links[predecesseur].tatt1 = -links[predecesseur].services[links[predecesseur].service].hf - links[predecesseur].services[links[predecesseur].service].delta * 1440f + links[pivot].h;
                                                        }
                                                        else
                                                        {
                                                            links[predecesseur].tatt1 = links[pivot].tatt1;

                                                        }
                                                        links[predecesseur].tatt = links[pivot].tatt - links[predecesseur].services[links[predecesseur].service].hf - links[predecesseur].services[links[predecesseur].service].delta * 1440f + links[pivot].h;
                                                        links[predecesseur].tveh = links[pivot].tveh + links[predecesseur].services[links[predecesseur].service].hf - links[predecesseur].services[links[predecesseur].service].hd;
                                                        links[predecesseur].tcor = links[pivot].tcor;
                                                        links[predecesseur].ncorr = links[pivot].ncorr;
                                                        links[predecesseur].tmap = links[pivot].tmap;
                                                        links[predecesseur].ttoll = links[pivot].ttoll + links[predecesseur].toll;
                                                        if (av_set) { links[predecesseur].cout = av_cout; }
                                                    if (tpen != 0f || tpc != 0f) { links[predecesseur].ttoll += tpen; links[predecesseur].cout += tpc; if (tpc < 0f && links[predecesseur].cout < links[pivot].cout) links[predecesseur].cout = links[pivot].cout; }

                                                        links[predecesseur].l = links[pivot].l + links[predecesseur].longueur;                                                //bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[predecesseur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                        bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                        while (bucket >= gga_nq.Count)
                                                        {
                                                            gga_nq.Add(new List<int>());
                                                        }
                                                        gga_nq[bucket].Add(predecesseur);
                                                        projet.param_affectation_horaire.nb_pop++;
                                                        //                                        touches.Enqueue(successeur);
                                                        links[predecesseur].pivot = pivot;
                                                        links[predecesseur].turn_pivot = j;
                                                        links[predecesseur].pole = links[pivot].pole;
                                                        links[predecesseur].poleV2 = "|" + nodes[links[predecesseur].nd].i + links[pivot].poleV2;
                                                    }
                                                }

                                            }


                                            //eléments déjà touchés
                                            else if (links[predecesseur].touche == 1 || links[predecesseur].touche == 2)
                                            {
                                                //bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[predecesseur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));


                                                //successeurs marche à pied pivot MAP

                                                if (links[predecesseur].ligne < 0 && links[pivot].ligne < 0 && projet.param_affectation_horaire.fcmap[pred_type] > 0 && (links[pivot].tmap + links[predecesseur].temps < projet.param_affectation_horaire.tmapmax) && links[predecesseur].cout > links[pivot].cout)
                                                {
                                                    bool test_periode = false;
                                                    int id_service = -1;
                                                    if (links[predecesseur].services.Count > 0)
                                                    {
                                                        int decal_jour = (int)(Math.Floor((links[pivot].h - penalite) / 1440f));
                                                        for (int kk = 0; kk < links[predecesseur].services.Count; kk++)
                                                        {
                                                            if (Math.Abs(decal_jour) <= projet.param_affectation_horaire.nb_jours)
                                                            {
                                                                if (projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[kk].regime][jour + decal_jour] == 'O' && links[predecesseur].services[kk].hd + 1440f * decal_jour <= links[pivot].h - penalite && links[predecesseur].services[kk].hf + 1440f * decal_jour > links[pivot].h - penalite)
                                                                {
                                                                    test_periode = true;
                                                                    id_service = kk;
                                                                }
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        test_periode = true;
                                                    }

                                                    if (test_periode == true)
                                                    {


                                                        if (links[predecesseur].cout - tpc > links[pivot].cout + (penalite + links[predecesseur].temps) * projet.param_affectation_horaire.fcoef_tmap[pred_type] * projet.param_affectation_horaire.fcmap[pred_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type])
                                                        {

                                                            bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                            gga_nq[bucket].Remove(predecesseur);
                                                            links[predecesseur].cout = links[pivot].cout + (penalite + links[predecesseur].temps) * projet.param_affectation_horaire.fcoef_tmap[pred_type] * projet.param_affectation_horaire.fcmap[pred_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type];
                                                            links[predecesseur].h = links[pivot].h - (links[predecesseur].temps) * projet.param_affectation_horaire.fcoef_tmap[pred_type] - penalite;
                                                            links[predecesseur].tatt = links[pivot].tatt;
                                                            links[predecesseur].tatt1 = links[pivot].tatt1;
                                                            links[predecesseur].tveh = links[pivot].tveh;
                                                            links[predecesseur].tcor = links[pivot].tcor;
                                                            links[predecesseur].ncorr = links[pivot].ncorr;
                                                            links[predecesseur].tmap = links[pivot].tmap + (penalite + links[predecesseur].temps) * projet.param_affectation_horaire.fcoef_tmap[pred_type];
                                                            links[predecesseur].ttoll = links[pivot].ttoll + links[predecesseur].toll;
                                                    if (tpen != 0f || tpc != 0f) { links[predecesseur].ttoll += tpen; links[predecesseur].cout += tpc; if (tpc < 0f && links[predecesseur].cout < links[pivot].cout) links[predecesseur].cout = links[pivot].cout; }

                                                            links[predecesseur].l = links[pivot].l + links[predecesseur].longueur;
                                                            links[predecesseur].touche = 2;

                                                            links[predecesseur].pivot = pivot;
                                                            links[predecesseur].turn_pivot = j;
                                                            links[predecesseur].pole = links[pivot].pole;
                                                            links[predecesseur].poleV2 = links[pivot].poleV2;

                                                            links[predecesseur].service = id_service;

                                                            //bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[predecesseur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                            bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                            gga_nq[bucket].Add(predecesseur);
                                                            projet.param_affectation_horaire.nb_pop++;

                                                        }
                                                    }

                                                }
                                                //predecesseurs marche à pied pivot TC
                                                else if (links[predecesseur].ligne < 0 && links[pivot].ligne > 0 && projet.param_affectation_horaire.fcmap[pred_type] > 0 && (links[pivot].tmap + links[predecesseur].temps < projet.param_affectation_horaire.tmapmax) && links[predecesseur].cout > links[pivot].cout)
                                                {
                                                    int id_service = -1;
                                                    bool test_periode = false;

                                                    if (links[predecesseur].services.Count > 0)
                                                    {
                                                        int decal_jour = -(int)(Math.Floor((links[pivot].h - temps_correspondance) / 1440f));
                                                        for (int kk = 0; kk < links[predecesseur].services.Count; kk++)
                                                        {
                                                            if (decal_jour <= projet.param_affectation_horaire.nb_jours)
                                                            {
                                                                if (projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[kk].regime][jour - decal_jour] == 'O' && links[predecesseur].services[kk].hd - 1440f * decal_jour <= links[pivot].h - penalite - temps_correspondance && links[predecesseur].services[kk].hf - 1440f * decal_jour > links[pivot].h - penalite - temps_correspondance)
                                                                {
                                                                    test_periode = true;
                                                                    id_service = kk;

                                                                }
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        test_periode = true;
                                                    }

                                                    if (test_periode == true)
                                                    {


                                                        if (links[predecesseur].cout - tpc > links[pivot].cout + (penalite + links[predecesseur].temps) * projet.param_affectation_horaire.fcoef_tmap[pred_type] * projet.param_affectation_horaire.fcmap[pred_type] + projet.param_affectation_horaire.fcboa[pivot_type] * temps_correspondance + projet.param_affectation_horaire.fcwait[pred_type] * temps_correspondance + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type])
                                                        {
                                                            bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                            gga_nq[bucket].Remove(predecesseur);
                                                            links[predecesseur].cout = links[pivot].cout + (penalite + links[predecesseur].temps) * projet.param_affectation_horaire.fcoef_tmap[pred_type] * projet.param_affectation_horaire.fcmap[pred_type] + projet.param_affectation_horaire.fcboa[pivot_type] * temps_correspondance + projet.param_affectation_horaire.fcwait[pred_type] * temps_correspondance + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type];
                                                            links[predecesseur].h = links[pivot].h - (links[predecesseur].temps) * projet.param_affectation_horaire.fcoef_tmap[pred_type] - temps_correspondance - penalite;
                                                            links[predecesseur].tatt = links[pivot].tatt + temps_correspondance;
                                                            links[predecesseur].tatt1 = links[pivot].tatt1;
                                                            links[predecesseur].tveh = links[pivot].tveh;
                                                            links[predecesseur].tcor = links[pivot].tcor + temps_correspondance;
                                                            links[predecesseur].ncorr = links[pivot].ncorr + 1;
                                                            links[predecesseur].l = links[pivot].l + links[predecesseur].longueur;
                                                            links[predecesseur].tmap = links[pivot].tmap + (penalite + links[predecesseur].temps) * projet.param_affectation_horaire.fcoef_tmap[pred_type];
                                                            links[predecesseur].ttoll = links[pivot].ttoll + links[predecesseur].toll;
                                                    if (tpen != 0f || tpc != 0f) { links[predecesseur].ttoll += tpen; links[predecesseur].cout += tpc; if (tpc < 0f && links[predecesseur].cout < links[pivot].cout) links[predecesseur].cout = links[pivot].cout; }

                                                            links[predecesseur].touche = 2;

                                                            links[predecesseur].pivot = pivot;
                                                            links[predecesseur].pole = nodes[links[predecesseur].nd].i;
                                                            links[predecesseur].poleV2 = "|" + nodes[links[predecesseur].nd].i + links[pivot].poleV2;

                                                            links[predecesseur].turn_pivot = j;

                                                            links[predecesseur].service = id_service;
                                                            //bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[predecesseur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                            bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                            gga_nq[bucket].Add(predecesseur);
                                                            projet.param_affectation_horaire.nb_pop++;

                                                        }
                                                    }

                                                }
                                                //successeurs TC même ligne
                                                else if ((links[predecesseur].ligne == links[pivot].ligne && projet.param_affectation_horaire.fcveh[pred_type] > 0 && links[pivot].ligne > 0) && (links[predecesseur].cout > links[pivot].cout))
                                                {
                                                    int ii, num_service = -1;
                                                    for (ii = 0; ii < links[predecesseur].services.Count; ii++)
                                                    {
                                                        if (links[predecesseur].services[ii].numero == links[pivot].services[links[pivot].service].numero)
                                                        {
                                                            if (links[predecesseur].services[ii].hf <= links[pivot].services[links[pivot].service].hd)
                                                            {
                                                                num_service = ii;
                                                            }
                                                        }


                                                    }

                                                    if (num_service != -1)
                                                    {
                                                        if (links[predecesseur].services[num_service].hf + links[pivot].services[links[pivot].service].delta * 1440f <= links[pivot].h)
                                                        {

                                                            if (links[predecesseur].cout - tpc > links[pivot].cout + (-links[predecesseur].services[num_service].hd - links[pivot].services[links[pivot].service].delta * 1440f + links[pivot].h) * projet.param_affectation_horaire.fcveh[pred_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type] && (links[predecesseur].services[links[predecesseur].service].hf <= links[pivot].services[links[pivot].service].hd))
                                                            {
                                                                bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                                gga_nq[bucket].Remove(predecesseur);
                                                                links[predecesseur].service = num_service;
                                                                links[predecesseur].services[num_service].delta = links[pivot].services[links[pivot].service].delta;

                                                                links[predecesseur].touche = 2;
                                                                links[predecesseur].cout = links[pivot].cout + (-links[predecesseur].services[links[predecesseur].service].hd - links[predecesseur].services[links[predecesseur].service].delta * 1440f + links[pivot].h) * projet.param_affectation_horaire.fcveh[pred_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type];
                                                                links[predecesseur].h = links[predecesseur].services[links[predecesseur].service].hd + links[predecesseur].services[links[predecesseur].service].delta * 60f * 24f;
                                                                links[predecesseur].tatt = links[pivot].tatt;
                                                                links[predecesseur].tatt1 = links[pivot].tatt1;
                                                                links[predecesseur].tveh = links[pivot].tveh - links[predecesseur].services[links[predecesseur].service].hd /*- links[predecesseur].services[links[predecesseur].service].delta * 1440f*/ + links[pivot].h;
                                                                links[predecesseur].tcor = links[pivot].tcor;
                                                                links[predecesseur].ncorr = links[pivot].ncorr;
                                                                links[predecesseur].l = links[pivot].l + links[predecesseur].longueur;
                                                                links[predecesseur].tmap = links[pivot].tmap;
                                                                links[predecesseur].ttoll = links[pivot].ttoll + links[predecesseur].toll;
                                                    if (tpen != 0f || tpc != 0f) { links[predecesseur].ttoll += tpen; links[predecesseur].cout += tpc; if (tpc < 0f && links[predecesseur].cout < links[pivot].cout) links[predecesseur].cout = links[pivot].cout; }

                                                                links[predecesseur].pivot = pivot;
                                                                links[predecesseur].turn_pivot = j;
                                                                links[predecesseur].pole = links[pivot].pole;
                                                                links[predecesseur].poleV2 = links[pivot].poleV2;

                                                                //bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[predecesseur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                                bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                                gga_nq[bucket].Add(predecesseur);
                                                                projet.param_affectation_horaire.nb_pop++;
                                                            }
                                                        }
                                                    }
                                                }
                                                //successeurs TC lignes différentes
                                                else if ((links[predecesseur].ligne != links[pivot].ligne && projet.param_affectation_horaire.fcveh[pred_type] > 0 && links[pivot].ligne > 0 && links[predecesseur].ligne > 0) && (links[predecesseur].cout > links[pivot].cout))
                                                {
                                                    int ii, jj, num_service = -1, h3 = -1, delta, duree_periode;
                                                    float h1 = 1e38f, h2 = 1e38f, cout2 = 1e38f;
                                                    for (ii = 0; ii < links[predecesseur].services.Count; ii++)
                                                    {
                                                        delta = 0;
                                                        duree_periode = projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[ii].regime].Length;

                                                        if (links[predecesseur].services[ii].hf + temps_correspondance > links[pivot].h || projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[ii].regime][jour] == 'N')
                                                        {

                                                            h1 = -1e38f;
                                                            h2 = 1e38f;
                                                            h3 = -1;
                                                            for (jj = jour - 1; jj >= Math.Max(jour - projet.param_affectation_horaire.nb_jours, 0); jj--)
                                                            {
                                                                if (projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[ii].regime][jj] == 'O' && (links[predecesseur].services[ii].hf + (-jour + jj) * 24f * 60f) > h1 && (links[predecesseur].services[ii].hf + (-jour + jj) * 24f * 60f) + temps_correspondance < links[pivot].h)
                                                                {
                                                                    h1 = links[predecesseur].services[ii].hf + (-jour + jj) * 24f * 60f;
                                                                    h2 = (-jour + jj);
                                                                    h3 = jj;
                                                                }

                                                            }
                                                            if (h3 != -1)
                                                            {
                                                                if (links[predecesseur].services[ii].delta > h2 || links[predecesseur].touche == 0)
                                                                {
                                                                    links[predecesseur].services[ii].delta = h2;
                                                                }

                                                            }
                                                            else
                                                            {
                                                                delta = 1;
                                                            }


                                                        }
                                                        if ((links[predecesseur].services[ii].hf + links[predecesseur].services[ii].delta * 1440f + max_correspondance >= links[pivot].h) && (links[predecesseur].services[ii].hf + links[predecesseur].services[ii].delta * 1440f + temps_correspondance <= links[pivot].h))

                                                        {
                                                            if (links[pivot].cout + (links[predecesseur].services[ii].hf - links[predecesseur].services[ii].hd) * projet.param_affectation_horaire.fcveh[pred_type] + (-links[predecesseur].services[ii].hf - links[predecesseur].services[ii].delta * 60f * 24f + links[pivot].h) * projet.param_affectation_horaire.fcwait[pred_type] + (temps_correspondance * projet.param_affectation_horaire.fcboa[pivot_type]) + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type] < cout2 && delta < 1)
                                                            {
                                                                cout2 = links[pivot].cout + (links[predecesseur].services[ii].hf - links[predecesseur].services[ii].hd) * projet.param_affectation_horaire.fcveh[pred_type] + (-links[predecesseur].services[ii].hf - links[predecesseur].services[ii].delta * 60f * 24f + links[pivot].h) * projet.param_affectation_horaire.fcwait[pred_type] + (temps_correspondance * projet.param_affectation_horaire.fcboa[pivot_type]) + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type];
                                                                num_service = ii;
                                                            }
                                                        }

                                                    }

                                                    if (num_service != -1)
                                                    {
                                                        if (links[predecesseur].cout - tpc > links[pivot].cout + (links[predecesseur].services[num_service].hf - links[predecesseur].services[num_service].hd) * projet.param_affectation_horaire.fcveh[pred_type] + (-links[predecesseur].services[num_service].hf - links[predecesseur].services[num_service].delta * 1440f + links[pivot].h) * projet.param_affectation_horaire.fcwait[pred_type] + (temps_correspondance * projet.param_affectation_horaire.fcboa[pivot_type]) + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type])
                                                        {
                                                            bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                            gga_nq[bucket].Remove(predecesseur);
                                                            links[predecesseur].service = num_service;
                                                            links[predecesseur].cout = links[pivot].cout + (links[predecesseur].services[num_service].hf - links[predecesseur].services[num_service].hd) * projet.param_affectation_horaire.fcveh[pred_type] + (-links[predecesseur].services[num_service].hf - links[predecesseur].services[num_service].delta * 1440f + links[pivot].h) * projet.param_affectation_horaire.fcwait[pred_type] + (temps_correspondance * projet.param_affectation_horaire.fcboa[pivot_type]) + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type];
                                                            links[predecesseur].touche = 2;

                                                            links[predecesseur].h = links[predecesseur].services[links[predecesseur].service].hd + links[predecesseur].services[links[predecesseur].service].delta * 60f * 24f;
                                                            if (links[pivot].tatt1 == 0)
                                                            {
                                                                links[predecesseur].tatt1 = -links[predecesseur].services[links[predecesseur].service].hf - links[predecesseur].services[links[predecesseur].service].delta * 1440f + links[pivot].h;
                                                            }
                                                            else
                                                            {
                                                                links[predecesseur].tatt1 = links[pivot].tatt1;

                                                            }
                                                            links[predecesseur].tatt = links[pivot].tatt - links[predecesseur].services[links[predecesseur].service].hf - links[predecesseur].services[links[predecesseur].service].delta * 1440f + links[pivot].h;
                                                            links[predecesseur].tveh = links[pivot].tveh + links[predecesseur].services[links[predecesseur].service].hf - links[predecesseur].services[links[predecesseur].service].hd;
                                                            links[predecesseur].tcor = links[pivot].tcor + temps_correspondance;
                                                            links[predecesseur].ncorr = links[pivot].ncorr + 1;
                                                            links[predecesseur].l = links[pivot].l + links[predecesseur].longueur;
                                                            links[predecesseur].tmap = links[pivot].tmap;
                                                            links[predecesseur].ttoll = links[pivot].ttoll + links[predecesseur].toll;
                                                    if (tpen != 0f || tpc != 0f) { links[predecesseur].ttoll += tpen; links[predecesseur].cout += tpc; if (tpc < 0f && links[predecesseur].cout < links[pivot].cout) links[predecesseur].cout = links[pivot].cout; }

                                                            links[predecesseur].pivot = pivot;
                                                            links[predecesseur].turn_pivot = j;
                                                            links[predecesseur].pole = nodes[links[predecesseur].nd].i;
                                                            links[predecesseur].poleV2 = "|" + nodes[links[predecesseur].nd].i + "|" + nodes[links[predecesseur].nd].i + links[pivot].poleV2;

                                                            //bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[predecesseur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                            bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                            gga_nq[bucket].Add(predecesseur);
                                                            projet.param_affectation_horaire.nb_pop++;
                                                        }
                                                    }

                                                }
                                                //predecesseurs TC lignes différentes pivot MAP
                                                else if ((links[predecesseur].ligne > 0 && links[predecesseur].ligne != links[pivot].ligne && projet.param_affectation_horaire.fcveh[pred_type] > 0 && links[pivot].ligne < 0) && (links[predecesseur].cout > links[pivot].cout))
                                                {
                                                    int ii, jj, num_service = -1, h3 = -1, delta, duree_periode;
                                                    float h1 = 1e38f, h2 = 1e38f, cout2 = 1e38f;
                                                    for (ii = 0; ii < links[predecesseur].services.Count; ii++)
                                                    {
                                                        delta = 0;

                                                        duree_periode = projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[ii].regime].Length;

                                                        if (links[predecesseur].services[ii].hf + temps_correspondance > links[pivot].h || projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[ii].regime][jour] == 'N')
                                                        {

                                                            h1 = -1e38f;
                                                            h2 = 1e38f;
                                                            h3 = -1;
                                                            for (jj = jour - 1; jj >= Math.Max(jour - projet.param_affectation_horaire.nb_jours, 0); jj--)
                                                            {
                                                                if (projet.reseaux[projet.reseau_actif].nom_calendrier[links[predecesseur].services[ii].regime][jj] == 'O' && (links[predecesseur].services[ii].hf + (-jour + jj) * 24f * 60f) > h1 && (links[predecesseur].services[ii].hf + (-jour + jj) * 24f * 60f) < links[pivot].h)
                                                                {
                                                                    h1 = links[predecesseur].services[ii].hf + (-jour + jj) * 24f * 60f;
                                                                    h2 = (-jour + jj);
                                                                    h3 = jj;
                                                                }

                                                            }
                                                            if (h3 != -1)
                                                            {
                                                                if (links[predecesseur].services[ii].delta > h2 || links[predecesseur].touche == 0)
                                                                {
                                                                    links[predecesseur].services[ii].delta = h2;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                delta = 1;
                                                            }


                                                        }
                                                        if ((links[predecesseur].services[ii].hf + links[predecesseur].services[ii].delta * 1440f + max_correspondance >= links[pivot].h) && (links[predecesseur].services[ii].hf + links[predecesseur].services[ii].delta * 1440f + temps_correspondance <= links[pivot].h))

                                                        {
                                                            if (links[pivot].cout + (links[predecesseur].services[ii].hf - links[predecesseur].services[ii].hd) * projet.param_affectation_horaire.fcveh[pred_type] + (-links[predecesseur].services[ii].hf - links[predecesseur].services[ii].delta * 60f * 24f + links[pivot].h) * projet.param_affectation_horaire.fcwait[pred_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type] < cout2 && delta < 1)
                                                            {
                                                                num_service = ii;
                                                                cout2 = links[pivot].cout + (links[predecesseur].services[ii].hf - links[predecesseur].services[ii].hd) * projet.param_affectation_horaire.fcveh[pred_type] + (-links[predecesseur].services[ii].hf - links[predecesseur].services[ii].delta * 60f * 24f + links[pivot].h) * projet.param_affectation_horaire.fcwait[pred_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type];
                                                            }
                                                        }
                                                    }

                                                    bool av_set = false, av_alt = false; float av_cout = 0f, av_delta = 0f; int av_idx = -1;
                                                    if (av_on && links[pivot].ncorr == 0)
                                                    {
                                                        AvArrivee(projet, links[predecesseur], jour, links[pivot].h, links[pivot].h, max_correspondance, links[pivot].cout, projet.param_affectation_horaire.fcveh[pred_type], projet.param_affectation_horaire.fcwait[pred_type], links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type], pa, pr, num_service, out av_alt, out av_idx, out av_delta, out av_cout);
                                                        av_set = (av_cout < 1e37f);
                                                        if (av_alt) { num_service = av_idx; links[predecesseur].services[av_idx].delta = av_delta; }
                                                    }
                                                    if (num_service != -1)
                                                    {
                                                        if (links[predecesseur].cout - tpc > (av_set ? av_cout : (links[pivot].cout + (links[predecesseur].services[num_service].hf - links[predecesseur].services[num_service].hd) * projet.param_affectation_horaire.fcveh[pred_type] + (-links[predecesseur].services[num_service].hf - links[predecesseur].services[num_service].delta * 1440f + links[pivot].h) * projet.param_affectation_horaire.fcwait[pred_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type])))
                                                        {

                                                            bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                            gga_nq[bucket].Remove(predecesseur);
                                                            links[predecesseur].service = num_service;
                                                            links[predecesseur].cout = links[pivot].cout + (links[predecesseur].services[num_service].hf - links[predecesseur].services[num_service].hd) * projet.param_affectation_horaire.fcveh[pred_type] + (-links[predecesseur].services[num_service].hf - links[predecesseur].services[num_service].delta * 1440f + links[pivot].h) * projet.param_affectation_horaire.fcwait[pred_type] + links[predecesseur].toll * projet.param_affectation_horaire.fctoll[pred_type];
                                                            links[predecesseur].touche = 2;

                                                            links[predecesseur].h = links[predecesseur].services[links[predecesseur].service].hd + links[predecesseur].services[links[predecesseur].service].delta * 60f * 24f;
                                                            if (links[pivot].tatt1 == 0)
                                                            {
                                                                links[predecesseur].tatt1 = -links[predecesseur].services[links[predecesseur].service].hf - links[predecesseur].services[links[predecesseur].service].delta * 1440f + links[pivot].h;
                                                            }
                                                            else
                                                            {
                                                                links[predecesseur].tatt1 = links[pivot].tatt1;

                                                            }
                                                            links[predecesseur].tatt = links[pivot].tatt - links[predecesseur].services[links[predecesseur].service].hf - links[predecesseur].services[links[predecesseur].service].delta * 1440f + links[pivot].h;
                                                            links[predecesseur].tveh = links[pivot].tveh + links[predecesseur].services[links[predecesseur].service].hf - links[predecesseur].services[links[predecesseur].service].hd;
                                                            links[predecesseur].tcor = links[pivot].tcor;
                                                            links[predecesseur].ncorr = links[pivot].ncorr;
                                                            links[predecesseur].tmap = links[pivot].tmap;
                                                            links[predecesseur].ttoll = links[pivot].ttoll + links[predecesseur].toll;
                                                        if (av_set) { links[predecesseur].cout = av_cout; }
                                                    if (tpen != 0f || tpc != 0f) { links[predecesseur].ttoll += tpen; links[predecesseur].cout += tpc; if (tpc < 0f && links[predecesseur].cout < links[pivot].cout) links[predecesseur].cout = links[pivot].cout; }

                                                            links[predecesseur].l = links[pivot].l + links[predecesseur].longueur;
                                                            links[predecesseur].pivot = pivot;
                                                            links[predecesseur].turn_pivot = j;
                                                            links[predecesseur].pole = links[pivot].pole;
                                                            links[predecesseur].poleV2 = "|" + nodes[links[predecesseur].nd].i + links[pivot].poleV2;
                                                            //bucket = (int)Math.Truncate(Math.Min((Math.Pow(links[predecesseur].cout, 2) / projet.param_affectation_horaire.param_dijkstra), projet.param_affectation_horaire.max_nb_buckets));
                                                            bucket = Convert.ToInt32(Math.Truncate(Math.Min(Math.Pow(links[predecesseur].cout / projet.param_affectation_horaire.param_dijkstra, projet.param_affectation_horaire.pu), projet.param_affectation_horaire.max_nb_buckets)));
                                                            gga_nq[bucket].Add(predecesseur);
                                                            projet.param_affectation_horaire.nb_pop++;
                                                        }
                                                    }
                                                }


                                            }

                                            /*List<int> liste = new List<int>();
                                            liste.Add(1289714);
                                            liste.Add(233502);
                                            liste.Add(233499);
                                            liste.Add(1289712);

                                            if (liste.Contains(pivot) || liste.Contains(predecesseur))
                                                {
                                                String texto = "pivot:" + nodes[links[pivot].no].i.ToString() + " " +
                                                   nodes[links[pivot].nd].i.ToString() + " " + 
                                                    links[pivot].h.ToString() +
                                                    " " + links[pivot].cout.ToString() +
                                                    " " + links[pivot].tmap.ToString()+
                                                    " " + links[pivot].tveh.ToString()  +
                                                    " " + links[pivot].tatt.ToString() + '\n' +
                                                    "pred:" + nodes[links[predecesseur].no].i.ToString() + " " +
                                                   nodes[links[predecesseur].nd].i.ToString() + 
                                                    " " + links[predecesseur].h.ToString() +
                                                    " " + links[predecesseur].cout.ToString() +
                                                    " " + links[predecesseur].tmap.ToString()+
                                                    " " + links[predecesseur].tveh.ToString() +
                                                    " " + links[predecesseur].tatt.ToString() +'\n'; 
                                                fich_log.Write(texto);
                                            }*/

                                        }
                                    }
                                    //links[pivot].touche = 3;
                                    //Console.WriteLine((touches.Count+calcules.Count).ToString());
                                }
                            fin_gga2:
                                //Console.WriteLine(p.ToString());

                                int arrivee = -1;
                                double cout_fin = 1e38f;
                                if (projet.reseaux[projet.reseau_actif].numnoeud.TryGetValue(p, out value) == true)
                                {
                                    for (j = 0; j < nodes[projet.reseaux[projet.reseau_actif].numnoeud[p]].SuccAll.Count; j++)
                                    {
                                        predecesseur = nodes[projet.reseaux[projet.reseau_actif].numnoeud[p]].SuccAll[j];
                                        if (links[predecesseur].touche != 0 && links[predecesseur].cout < cout_fin)
                                        {
                                            arrivee = predecesseur;
                                            cout_fin = links[predecesseur].cout;

                                        }




                                    }
                                }
                                else
                                {
                                    fich_log.WriteLine("OD error" + libod + ":" + ":" + chaine + ": non existing origin node!");
                                }

                                if (arrivee != -1)
                                {
                                    if (links[arrivee].ligne > 0)
                                    {
                                        links[arrivee].boai += od;
                                        links[arrivee].services[links[arrivee].service].boai = od;
                                        links[arrivee].services[links[arrivee].service].boat += od;
                                    }
                                }
                                else
                                {
                                    fich_log.WriteLine("OD error" + libod + ":" + chaine + ": unreachable origin node!");
                                }

                                pivot = arrivee;
                                string itineraire = "", texte;
                                if (pivot != -1)
                                {
                                    string[] param2 = { "|" }, lignes_corr = null;
                                    if (links[pivot].texte != null)
                                    {

                                        lignes_corr = links[pivot].texte.Split(param2, StringSplitOptions.RemoveEmptyEntries);
                                    }
                                    if (lignes_corr == null)
                                    {
                                        itineraire = "MAP";
                                    }
                                    else
                                    {
                                        itineraire = lignes_corr[0];
                                    }
                                }
                                while (pivot != -1)
                                {
                                    links[pivot].volau += od;
                                    if (links[pivot].pivot != -1 && projet.param_affectation_horaire.sortie_turns == true)
                                    {
                                        Turn virage = new Turn();
                                        virage.arci = pivot;
                                        virage.arcj = links[pivot].pivot;
                                        float value2;
                                        if (transfers.TryGetValue(virage, out value2) == true)
                                        {

                                            transfers[virage] += od;
                                        }
                                        else
                                        {
                                            transfers[virage] = od;
                                        }

                                        //links[links[pivot].pivot].arci[links[pivot].turn_pivot].volau += od;
                                    }
                                    if (links[pivot].service >= 0)
                                    {
                                        links[pivot].services[links[pivot].service].volau += od;
                                    }

                                    if (links[pivot].pivot == -1)
                                    {
                                        if (links[pivot].ligne > 0)
                                        {
                                            links[pivot].alij += od;
                                            links[pivot].services[links[pivot].service].alij = od;
                                            links[pivot].services[links[pivot].service].alit += od;

                                        }
                                    }
                                    else if (links[pivot].ligne != links[links[pivot].pivot].ligne)
                                    {
                                        if (links[pivot].ligne > 0)
                                        {
                                            links[pivot].alij += od;
                                            links[pivot].services[links[pivot].service].alij = od;
                                            links[pivot].services[links[pivot].service].alit += od;

                                        }
                                        if (links[links[pivot].pivot].ligne > 0)
                                        {
                                            links[links[pivot].pivot].boai += od;
                                            links[links[pivot].pivot].services[links[links[pivot].pivot].service].boai = od;
                                            links[links[pivot].pivot].services[links[links[pivot].pivot].service].boat += od;
                                        }

                                    }
                                    if (projet.param_affectation_horaire.sortie_chemins == true)
                                    {
                                        texte = libod + ";" + p + ";" + q + ";" + jour.ToString("0") + ";" + horaire.ToString("0.000");
                                        texte += ";" + nodes[links[pivot].no].i;
                                        texte += ";" + nodes[links[pivot].nd].i;
                                        texte += ";" + nodes[links[pivot].no].i + "-" + nodes[links[pivot].nd].i;

                                        texte += ";" + links[pivot].ligne.ToString("0");
                                        if (links[pivot].service >= 0)
                                        {
                                            texte += ";" + links[pivot].services[links[pivot].service].numero.ToString("0");
                                        }
                                        else
                                        {
                                            texte += ";-1";
                                        }
                                        texte += ";" + (-links[pivot].h + horaire).ToString("0.000");
                                        texte += ";" + links[pivot].h.ToString("0.000");
                                        texte += ";" + links[pivot].tveh.ToString("0.000");
                                        texte += ";" + links[pivot].tmap.ToString("0.000");
                                        texte += ";" + links[pivot].tatt.ToString("0.000");
                                        texte += ";" + links[pivot].tcor.ToString("0.000");
                                        texte += ";" + links[pivot].ncorr.ToString("0.000");
                                        texte += ";" + links[pivot].tatt1.ToString("0.000");
                                        texte += ";" + links[pivot].cout.ToString("0.000");
                                        texte += ";" + links[pivot].l.ToString("0.000");
                                        texte += ";" + links[pivot].pole;
                                        texte += ";" + od.ToString("0.00");
                                        if (links[pivot].ligne != -1)
                                        {
                                            texte += ";" + links[pivot].services[links[pivot].service].boai.ToString("0.000");
                                            texte += ";" + links[pivot].services[links[pivot].service].alij.ToString("0.000");
                                            links[pivot].services[links[pivot].service].boai = 0;
                                            links[pivot].services[links[pivot].service].alij = 0;

                                        }
                                        else
                                        {
                                            texte += ";0";
                                            texte += ";0";
                                        }
                                        texte += ";" + links[pivot].texte;
                                        texte += ";" + links[pivot].type;

                                        texte += ";" + links[pivot].ttoll.ToString("0.000");


                                        fich_sortie2.WriteLine(texte);



                                    }
                                    if (links[pivot].pivot != -1)
                                    {
                                        if (links[pivot].ligne != links[links[pivot].pivot].ligne)
                                        {
                                            string[] param2 = { "|" }, lignes_corr = null;
                                            if (links[pivot].texte != null)
                                            {


                                                lignes_corr = links[links[pivot].pivot].texte.Split(param2, StringSplitOptions.RemoveEmptyEntries);
                                            }
                                            if (lignes_corr == null)
                                            {
                                                itineraire = itineraire + "|MAP"; ;
                                            }
                                            else
                                            {
                                                itineraire = itineraire + "|" + lignes_corr[0];
                                            }
                                        }
                                    }
                                    pivot = links[pivot].pivot;
                                }
                                if (arrivee != -1)
                                {
                                    texte = libod + ";" + p + ";" + q;
                                    texte += ";" + jour.ToString("0.000");
                                    texte += ";" + links[arrivee].h.ToString("0.000");
                                    texte += ";" + horaire.ToString("0.000");
                                    texte += ";" + (horaire - links[arrivee].h).ToString("0.000");
                                    texte += ";" + links[arrivee].tveh.ToString("0.000");
                                    texte += ";" + links[arrivee].tmap.ToString("0.000");
                                    texte += ";" + links[arrivee].tatt.ToString("0.000");
                                    texte += ";" + links[arrivee].tcor.ToString("0.000");
                                    texte += ";" + links[arrivee].ncorr.ToString("0.000");
                                    texte += ";" + links[arrivee].tatt1.ToString("0.000");
                                    texte += ";" + links[arrivee].cout.ToString("0.000");
                                    texte += ";" + links[arrivee].l.ToString("0.000");
                                    texte += ";" + links[arrivee].pole;
                                    texte += ";" + od.ToString("0.00");
                                    //itineraire = "MAP," + itineraire;
                                    texte += ";" + itineraire;
                                    texte += ";" + projet.param_affectation_horaire.nb_pop;
                                    texte += ";" + links[arrivee].ttoll.ToString("0.000");



                                    fich_od.WriteLine(texte);

                                    if (projet.param_affectation_horaire.sortie_noeuds == true)
                                    {
                                        foreach (node n in nodes)
                                        {
                                            float tmax = 1e38f;
                                            String type_arc = "";
                                            int which_tmax = -1;
                                            link troncon = null;
                                            for (int s = 0; s < n.SuccAll.Count; s++)
                                            {
                                                troncon = links[n.SuccAll[s]];
                                                if (troncon.cout <= tmax && troncon.touche != 0 && (troncon.ligne <= 0 || projet.param_affectation_horaire.sortie_temps == 2))
                                                {
                                                    tmax = troncon.cout;
                                                    which_tmax = n.SuccAll[s];
                                                    type_arc = troncon.type;

                                                }

                                            }
                                            if (which_tmax >= 0 && tmax <= projet.param_affectation_horaire.temps_max)
                                            {
                                                if (filtre.Contains(type_arc) || filtre.Count == 0)
                                                {
                                                    if (projet.param_affectation_horaire.sortie_temps == 3)
                                                    {
                                                        texte = q;
                                                        texte += ";" + n.i;
                                                        texte += ";" + (horaire - links[which_tmax].h).ToString("0.000");
                                                        texte += ";" + links[which_tmax].tatt1.ToString("0.000");
                                                        texte += ";" + od.ToString("0.000");

                                                    }
                                                    else
                                                    {
                                                        texte = libod + ";" + p + ";" + q;
                                                        texte += ";" + jour.ToString("0.000");
                                                        texte += ";" + n.i;
                                                        texte += ";" + horaire.ToString("0.000");
                                                        texte += ";" + links[which_tmax].h.ToString("0.000");
                                                        texte += ";" + (horaire - links[which_tmax].h).ToString("0.000");
                                                        texte += ";" + links[which_tmax].tveh.ToString("0.000");
                                                        texte += ";" + links[which_tmax].tmap.ToString("0.000");
                                                        texte += ";" + links[which_tmax].tatt.ToString("0.000");
                                                        texte += ";" + links[which_tmax].tcor.ToString("0.000");
                                                        texte += ";" + links[which_tmax].ncorr.ToString("0");
                                                        texte += ";" + links[which_tmax].tatt1.ToString("0.000");
                                                        texte += ";" + links[which_tmax].cout.ToString("0.000");
                                                        texte += ";" + links[which_tmax].l.ToString("0.000");
                                                        texte += ";" + links[which_tmax].pole;
                                                        texte += ";" + links[which_tmax].ttoll.ToString("0.000");
                                                        texte += ";" + od.ToString("0.000");
                                                        if (projet.param_affectation_horaire.sortie_stops == true)
                                                        {
                                                            texte += ";" + links[which_tmax].poleV2;
                                                        }
                                                        else
                                                        {
                                                            texte += ";";
                                                        }
                                                    }
                                                    fich_noeuds.WriteLine(texte);
                                                }
                                            }
                                        }
                                    }
                                    if (projet.param_affectation_horaire.sortie_temps == 0)
                                    {
                                        network reseau = projet.reseaux[projet.reseau_actif];
                                        double som_detour = 0; double nb_detour = 0; double som_oiseau = 0;
                                        double d_oiseau, d_link;

                                        foreach (link li in reseau.links)
                                        {
                                            d_oiseau = Math.Pow(Math.Pow((reseau.nodes[reseau.numnoeud[q]].x - reseau.nodes[li.no].x), 2) + Math.Pow((reseau.nodes[reseau.numnoeud[q]].y - reseau.nodes[li.no].y), 2), 0.5);
                                            d_link = Math.Pow(Math.Pow((reseau.nodes[li.no].x - reseau.nodes[li.nd].x), 2) + Math.Pow((reseau.nodes[li.no].y - reseau.nodes[li.nd].y), 2), 0.5);

                                            if (d_oiseau > 500 & d_oiseau - d_link <= 500)
                                            {

                                                if (reseau.nodes[reseau.numnoeud[q]].x > 0 && reseau.nodes[reseau.numnoeud[q]].y > 0 && reseau.nodes[li.no].x > 0 && reseau.nodes[li.no].y > 0)
                                                {
                                                    if (li.h > 0)
                                                    {
                                                        som_detour += li.h;
                                                        som_oiseau += Math.Pow(Math.Pow((reseau.nodes[reseau.numnoeud[q]].x - reseau.nodes[li.no].x), 2) + Math.Pow((reseau.nodes[reseau.numnoeud[q]].y - reseau.nodes[li.no].y), 2), 0.5);
                                                        nb_detour++;
                                                    }
                                                }
                                            }
                                        }
                                        fich_detour.WriteLine(q + ";" + som_detour.ToString() + ";" + som_oiseau.ToString() + ";" + nb_detour.ToString());

                                    }


                                    if (projet.param_affectation_horaire.sortie_temps > 0 && projet.param_affectation_horaire.sortie_temps < 4)
                                    {
                                        for (i = 0; i < links.Count; i++)
                                        {

                                            if (links[i].orig >= 0) continue; arrivee = MeilleurEtat(links, i, w.cloB);
                                            if (filtre.Contains(links[arrivee].type) || filtre.Count == 0)
                                            {

                                                if (links[arrivee].touche != 0 && (links[arrivee].ligne < 0 || projet.param_affectation_horaire.sortie_temps == 2))
                                                {
                                                    if (projet.param_affectation_horaire.sortie_noeuds == false && projet.param_affectation_horaire.sortie_temps == 3)
                                                    {
                                                        texte = q;
                                                        texte += ";" + nodes[links[arrivee].no].i;
                                                        texte += "-" + nodes[links[arrivee].nd].i;
                                                        texte += ";" + (horaire - links[arrivee].h).ToString("0.000");
                                                        texte += ";" + links[arrivee].tatt1.ToString("0.000");
                                                        texte += ";" + od.ToString("0.000");


                                                    }
                                                    else if (projet.param_affectation_horaire.sortie_temps < 3)
                                                    {

                                                        texte = libod + ";" + q;
                                                        texte += ";" + nodes[links[arrivee].no].i;
                                                        texte += "-" + nodes[links[arrivee].nd].i;
                                                        texte += ";" + (links[arrivee].ligne).ToString("0");
                                                        texte += ";" + i.ToString("0");
                                                        texte += ";" + jour.ToString("0");


                                                        texte += ";" + links[arrivee].h.ToString("0.000");
                                                        texte += ";" + horaire.ToString("0.000");
                                                        link arc = null;
                                                        arc = links[arrivee];
                                                        float ti;
                                                        if (arc.ligne < 0)
                                                        {
                                                            ti = horaire - (arc.h + arc.temps * projet.param_affectation_horaire.fcoef_tmap[arc.type]);
                                                        }
                                                        else
                                                        {
                                                            ti = horaire - (arc.h + (arc.services[arc.service].hf - arc.services[arc.service].hd));
                                                        }
                                                        texte += ";" + ti.ToString("0.000");

                                                        texte += ";" + links[arrivee].tveh.ToString("0.000");
                                                        texte += ";" + links[arrivee].tmap.ToString("0.000");
                                                        texte += ";" + links[arrivee].tatt.ToString("0.000");
                                                        texte += ";" + links[arrivee].tcor.ToString("0.000");
                                                        texte += ";" + links[arrivee].ncorr.ToString("0.000");
                                                        texte += ";" + links[arrivee].tatt1.ToString("0.000");
                                                        texte += ";" + links[arrivee].cout.ToString("0.000");
                                                        texte += ";" + links[arrivee].l.ToString("0.000");
                                                        texte += ";" + links[arrivee].pole;
                                                        texte += ";" + od.ToString("0.00");
                                                        //texte += ";" + links[arrivee].texte;
                                                        /*texte += ";" + projet.param_affectation_horaire.texte_cveh;
                                                        texte += ";" + projet.param_affectation_horaire.texte_cwait;
                                                        texte += ";" + projet.param_affectation_horaire.texte_cmap;
                                                        texte += ";" + projet.param_affectation_horaire.texte_cboa;
                                                        texte += ";" + projet.param_affectation_horaire.texte_coef_tmap;
                                                        texte += ";" + projet.param_affectation_horaire.texte_tboa;
                                                        texte += ";" + projet.param_affectation_horaire.nb_jours;*/
                                                        texte += ";" + links[arrivee].pivot.ToString("0");
                                                        texte += ";" + links[arrivee].type;
                                                        texte += ";" + links[arrivee].ttoll.ToString("0.000");
                                                        texte += ";" + (horaire - links[arrivee].h).ToString("0.000");

                                                    }


                                                    //                                itineraire = "MAP," + itineraire;
                                                    if (links[arrivee].cout <= projet.param_affectation_horaire.temps_max)
                                                    {
                                                        if (test_temps_per_min_arrivee(projet, arrivee) == true)
                                                        {
                                                            if (!(projet.param_affectation_horaire.sortie_temps == 3 && projet.param_affectation_horaire.sortie_noeuds == true))
                                                            {
                                                                fich_sortie.WriteLine(texte);
                                                            }
                                                        }


                                                    }
                                                }
                                                else if (links[arrivee].touche == 0 && projet.param_affectation_horaire.sortie_isoles == true)
                                                {
                                                    texte = nodes[links[arrivee].no].i;
                                                    texte += "-" + nodes[links[arrivee].nd].i;
                                                    texte += ";" + (links[arrivee].ligne).ToString("0");
                                                    texte += ";" + i.ToString("0");
                                                    fich_isoles.WriteLine(texte);

                                                }
                                            }
                                        }
                                    }
                                }
                            }
        }

        public static bool test_temps_per_min_depart(etude projet, int arrivee)
        {

            bool reponse = true;
            link arc = new link();
            arc = projet.reseaux[projet.reseau_actif].links[arrivee];
            int ni;
            ni = arc.no;
            foreach (int k in projet.reseaux[projet.reseau_actif].nodes[ni].succ)
            {
                int nj = projet.reseaux[projet.reseau_actif].links[k].nd;
                int ligne = projet.reseaux[projet.reseau_actif].links[k].ligne;
                if ((nj == arc.nd) && !(arc.ligne == ligne) && projet.reseaux[projet.reseau_actif].links[k].touche != 0)
                {
                    if (arc.cout > projet.reseaux[projet.reseau_actif].links[k].cout)
                    {
                        reponse = false;
                    }
                }
            }
            return reponse;
        }
        public static bool test_temps_per_min_arrivee(etude projet, int arrivee)
        {

            bool reponse = true;
            link arc = new link();
            arc = projet.reseaux[projet.reseau_actif].links[arrivee];
            int nj;
            nj = arc.nd;
            foreach (int k in projet.reseaux[projet.reseau_actif].nodes[nj].pred)
            {
                int ni = projet.reseaux[projet.reseau_actif].links[k].no;
                int ligne = projet.reseaux[projet.reseau_actif].links[k].ligne;
                if ((ni == arc.no) && !(arc.ligne == ligne) && projet.reseaux[projet.reseau_actif].links[k].touche != 0)
                {
                    if (arc.cout > projet.reseaux[projet.reseau_actif].links[k].cout)
                    {
                        reponse = false;
                    }
                }
            }
            return reponse;
        }
    }
}