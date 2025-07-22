using LoveQuiz.Server.Models;
using System;
using System.Runtime.CompilerServices;

namespace LoveQuiz.Server.Services
{
    public static  class StaticInfo
    {
        public static readonly Dictionary<int, AttachmentStyleInfo> AttachmentStyles;
        public static readonly Dictionary<int, ToxicHabitsSection> ToxicHabitsByStyle;
        public static readonly List<EmotionalNeed> EmotionalNeeds;
        //ATTACHMENT
      
        //ATTACHEMENT STYLE ID
        private const int ANXIOUS_PREOCUPAT_ID = 1;
        private const int EVITANT_DEZANGAJAT_ID = 2;
        private const int PREOCUPAT_CONTROLATOR_ID = 3;
        private const int MANIPULATIV_DEFENSIV_ID = 4;
        private const int ECHILIBRAT_ID = 5;

        //ATTACHEMENT STYLE LABEL
        private const string ANXIOUS_PREOCUPAT_LABEL = "Anxios–preocupat";
        private const string EVITANT_DEZANGAJAT_LABEL = "Evitant–dezangajat";
        private const string PREOCUPAT_CONTROLATOR_LABEL = "Preocupat–controlator";
        private const string MANIPULATIV_DEFENSIV_LABEL = "Manipulativ–defensiv";
        private const string ECHILIBRAT_LABEL = "Echilibrat";

        //ATTACHEMENT STYLE NICNKAMES
        private const string ANXIOUS_PREOCUPAT_NICKNAME = "Furtuna Anxioasă";
        private const string EVITANT_DEZANGAJAT_NICKNAME = "Umbra Detașată";
        private const string PREOCUPAT_CONTROLATOR_NICKNAME = "Căpitanul Controlator";
        private const string MANIPULATIV_DEFENSIV_NICKNAME = "Zidul Strateg";
        private const string ECHILIBRAT_NICKNAME = "Ancora Calmă";

        //ATTACHEMENT STYLE SUMMARIES
        private const string ANXIOUS_PREOCUPAT_SUMMARY =
    "Dorești cu disperare apropierea și confirmarea iubirii, dar trăiești cu o frică constantă de respingere. " +
    "Orice întârziere în răspuns sau lipsă de afecțiune te face să te simți nesigur(ă). " +
    "Ai o sensibilitate crescută la semne de respingere și uneori poți părea sufocant(ă) sau copleșitor(oare). " +
    "Intențiile tale sunt sincere, dar intensitatea emoțională poate destabiliza relația.";

        private const string EVITANT_DEZANGAJAT_SUMMARY =
            "Te retragi emoțional când lucrurile devin intense. " +
            "Eviți discuțiile profunde sau conflictele, preferând să păstrezi distanța. " +
            "Independența este esențială pentru tine, dar asta poate fi perceput ca dezinteres. " +
            "Partenerii tăi se pot simți respinși sau neiubiți, chiar dacă tu nu intenționezi asta.";

        private const string PREOCUPAT_CONTROLATOR_SUMMARY =
            "Îți dorești apropiere și siguranță, dar încerci să le obții controlând relația. " +
            "Te implici intens și vrei să gestionezi totul pentru a evita incertitudinea. " +
            "Îți este greu să lași lucrurile să curgă natural, iar asta poate sufoca partenerul. " +
            "Intenția ta este de a proteja relația, dar metoda poate crea tensiune.";

        private const string MANIPULATIV_DEFENSIV_SUMMARY =
            "Ai dificultăți în a te exprima direct și sincer în relație. " +
            "Poți recurge la sarcasm, pasiv-agresivitate sau evitare atunci când ești rănit(ă). " +
            "Ai învățat să te protejezi prin mecanisme de apărare care pot părea manipulatoare. " +
            "Sub toate acestea există o nevoie profundă de siguranță și acceptare.";

        private const string ECHILIBRAT_SUMMARY =
            "Te simți confortabil(ă) atât cu intimitatea, cât și cu spațiul personal. " +
            "Poți comunica deschis, gestiona conflictele sănătos și oferi siguranță emoțională. " +
            "Nu te temi de vulnerabilitate și știi cum să construiești o relație hrănitoare. " +
            "Ai un stil de atașament sănătos care favorizează conexiunea autentică.";

        //TOXIC HABITS

        //1:anxious preoccupied
        public const string ANXIOS_PREOCUPAT_TITLE = "Toxicitate";
        public const string ANXIOS_PREOCUPAT_SUBTITLE = "🔸 Te agăți emoțional, mereu în căutarea reasigurării.";

        public const string ANXIOS_PREOCUPAT_SUBTITLE_1 = "Teama constantă de abandon";
        public const string ANXIOS_PREOCUPAT_DESCRIPTION_1 = "Te temi că vei fi părăsit(ă) și ceri confirmări exagerate.";

        public const string ANXIOS_PREOCUPAT_SUBTITLE_2 = "Supraanalizarea mesajelor";
        public const string ANXIOS_PREOCUPAT_DESCRIPTION_2 = "Interpretezi excesiv gesturile și cuvintele partenerului, generând tensiuni.";

        public const string ANXIOS_PREOCUPAT_SUBTITLE_3 = "Dependență emoțională";
        public const string ANXIOS_PREOCUPAT_DESCRIPTION_3 = "Ai nevoie de atenție continuă pentru a te simți iubit(ă).";

        public const string ANXIOS_PREOCUPAT_SUBTITLE_4 = "Crize de gelozie";
        public const string ANXIOS_PREOCUPAT_DESCRIPTION_4 = "Reacționezi intens la semnale minime de amenințare relațională.";

        //2:evitant dezangajat
        public const string EVITANT_DEZANGAJAT_TITLE = "Deconectare emoțională";
        public const string EVITANT_DEZANGAJAT_SUBTITLE = "🔸 Te retragi emoțional și fugi de vulnerabilitate.";

        public const string EVITANT_DEZANGAJAT_SUBTITLE_1 = "Evitarea conflictelor prin tăcere";
        public const string EVITANT_DEZANGAJAT_DESCRIPTION_1 = "Eviți discuțiile serioase și lași partenerul în incertitudine.";

        public const string EVITANT_DEZANGAJAT_SUBTITLE_2 = "Minimizarea nevoilor partenerului";
        public const string EVITANT_DEZANGAJAT_DESCRIPTION_2 = "Tratezi cu superficialitate dorințele emoționale ale celuilalt.";

        public const string EVITANT_DEZANGAJAT_SUBTITLE_3 = "Independență dusă la extrem";
        public const string EVITANT_DEZANGAJAT_DESCRIPTION_3 = "Refuzi apropierea și sprijinul, considerându-le slăbiciuni.";

        public const string EVITANT_DEZANGAJAT_SUBTITLE_4 = "Răceală afectivă";
        public const string EVITANT_DEZANGAJAT_DESCRIPTION_4 = "Te protejezi prin detașare chiar și în momentele în care conexiunea contează.";

        //3:preocupat controlator 
        public const string PREOCUPAT_CONTROLATOR_TITLE = "Conflict de atașament";
        public const string PREOCUPAT_CONTROLATOR_SUBTITLE = "🔸 Controlezi pentru a nu fi rănit(ă).";

        public const string PREOCUPAT_CONTROLATOR_SUBTITLE_1 = "Întrebări obsesive";
        public const string PREOCUPAT_CONTROLATOR_DESCRIPTION_1 = "Pui întrebări excesive pentru a simți că deții controlul.";

        public const string PREOCUPAT_CONTROLATOR_SUBTITLE_2 = "Monitorizare a partenerului";
        public const string PREOCUPAT_CONTROLATOR_DESCRIPTION_2 = "Verifici frecvent partenerul, suspectând trădări sau ascunzișuri.";

        public const string PREOCUPAT_CONTROLATOR_SUBTITLE_3 = "Șantaj emoțional";
        public const string PREOCUPAT_CONTROLATOR_DESCRIPTION_3 = "Folosești vinovăția sau amenințările subtile pentru a obține ce vrei.";

        public const string PREOCUPAT_CONTROLATOR_SUBTITLE_4 = "Neîncredere profundă";
        public const string PREOCUPAT_CONTROLATOR_DESCRIPTION_4 = "Presupui mereu că există intenții ascunse, ceea ce creează tensiune.";

        //4: manipulativ defensiv
        public const string MANIPULATIV_DEFENSIV_TITLE = "Ciclu de conflict";
        public const string MANIPULATIV_DEFENSIV_SUBTITLE = "🔸 Pare că te aperi, dar în esență manipulezi pentru a evita durerea.";

        public const string MANIPULATIV_DEFENSIV_SUBTITLE_1 = "Victimizare";
        public const string MANIPULATIV_DEFENSIV_DESCRIPTION_1 = "Te prezinți ca victimă pentru a evita responsabilitatea personală.";

        public const string MANIPULATIV_DEFENSIV_SUBTITLE_2 = "Pasiv–agresivitate";
        public const string MANIPULATIV_DEFENSIV_DESCRIPTION_2 = "Spui că ești bine, dar transmiți supărarea prin tăceri și gesturi subtile.";

        public const string MANIPULATIV_DEFENSIV_SUBTITLE_3 = "Distorsionarea adevărului";
        public const string MANIPULATIV_DEFENSIV_DESCRIPTION_3 = "Omite fapte sau le modifici pentru a controla emoțional situația.";

        public const string MANIPULATIV_DEFENSIV_SUBTITLE_4 = "Refuzul asumării greșelilor";
        public const string MANIPULATIV_DEFENSIV_DESCRIPTION_4 = "Te aperi imediat sau dai vina pe altcineva când ești confruntat(ă).";

        //5:echilibrat
        public const string ECHILIBRAT_TITLE = "Intimitate neglijată";
        public const string ECHILIBRAT_SUBTITLE = "🔸 Ești echilibrat(ă), dar uneori ignori aspecte esențiale ale intimității.";

        public const string ECHILIBRAT_SUBTITLE_1 = "Ignorarea propriei oboseli emoționale";
        public const string ECHILIBRAT_DESCRIPTION_1 = "Te concentrezi prea mult pe partener și uiți să ai grijă de tine.";

        public const string ECHILIBRAT_SUBTITLE_2 = "Tendința de a evita conflictele";
        public const string ECHILIBRAT_DESCRIPTION_2 = "Eviți tensiunile pentru a păstra armonia, dar îți reprimi nevoile reale.";

        public const string ECHILIBRAT_SUBTITLE_3 = "Perfecționism relațional";
        public const string ECHILIBRAT_DESCRIPTION_3 = "Ai așteptări înalte de la relație și devii critic(ă) când realitatea nu se aliniază.";

        public const string ECHILIBRAT_SUBTITLE_4 = "Nevoia de validare excesivă";
        public const string ECHILIBRAT_DESCRIPTION_4 = "Cauți confirmare frecventă chiar și atunci când nu e necesar.";


        //10 EMOTIONAL NEEDS
        private const string EMOTIONAL_NEED_TITLE_1 = "Să fii înțeles(ă)";
        private const string EMOTIONAL_NEED_DESCRIPTION_1 = "Să simți că partenerul te ascultă cu adevărat și îți înțelege trăirile.";

        private const string EMOTIONAL_NEED_TITLE_2 = "Să te simți în siguranță";
        private const string EMOTIONAL_NEED_DESCRIPTION_2 = "Să știi că poți fi tu însuți/însăți fără frică de judecată sau abandon.";

        private const string EMOTIONAL_NEED_TITLE_3 = "Să fii valorizat(ă)";
        private const string EMOTIONAL_NEED_DESCRIPTION_3 = "Să simți că ceea ce ești și ceea ce faci contează pentru partener.";

        private const string EMOTIONAL_NEED_TITLE_4 = "Responsivitate emoțională";
        private const string EMOTIONAL_NEED_DESCRIPTION_4 = "Ca partenerul să reacționeze cu empatie atunci când ai nevoie.";

        private const string EMOTIONAL_NEED_TITLE_5 = "Încredere";
        private const string EMOTIONAL_NEED_DESCRIPTION_5 = "Să te poți baza pe celălalt și să știi că este acolo pentru tine.";

        private const string EMOTIONAL_NEED_TITLE_6 = "Sprijin în vulnerabilitate";
        private const string EMOTIONAL_NEED_DESCRIPTION_6 = "Să poți fi deschis(ă) și rănit(ă) fără a fi atacat(ă) sau ignorat(ă).";

        private const string EMOTIONAL_NEED_TITLE_7 = "Afecțiune & atingere";
        private const string EMOTIONAL_NEED_DESCRIPTION_7 = "Gesturi fizice care exprimă iubire și conectare (îmbrățișări, mângâieri).";

        private const string EMOTIONAL_NEED_TITLE_8 = "Autonomie & spațiu personal";
        private const string EMOTIONAL_NEED_DESCRIPTION_8 = "Libertatea de a avea timp pentru tine fără vină sau presiune.";

        private const string EMOTIONAL_NEED_TITLE_9 = "Claritate în comunicare";
        private const string EMOTIONAL_NEED_DESCRIPTION_9 = "Dialog sincer, direct, fără ambiguitate sau „ghiceli”.";

        private const string EMOTIONAL_NEED_TITLE_10 = "Scop comun";
        private const string EMOTIONAL_NEED_DESCRIPTION_10 = "Să simți că mergeți în aceeași direcție, cu planuri și valori împărtășite.";



        static StaticInfo()
        {
            AttachmentStyles = new Dictionary<int, AttachmentStyleInfo>
    {
        {
            ANXIOUS_PREOCUPAT_ID,
            new AttachmentStyleInfo
            {
                Id = ANXIOUS_PREOCUPAT_ID,
                Label = ANXIOUS_PREOCUPAT_LABEL,
                Nickname = ANXIOUS_PREOCUPAT_NICKNAME,
                Summary = ANXIOUS_PREOCUPAT_SUMMARY
            }
        },
        {
            EVITANT_DEZANGAJAT_ID,
            new AttachmentStyleInfo
            {
                Id = EVITANT_DEZANGAJAT_ID,
                Label = EVITANT_DEZANGAJAT_LABEL,
                Nickname = EVITANT_DEZANGAJAT_NICKNAME,
                Summary = EVITANT_DEZANGAJAT_SUMMARY
            }
        },
        {
            PREOCUPAT_CONTROLATOR_ID,
            new AttachmentStyleInfo
            {
                Id = PREOCUPAT_CONTROLATOR_ID,
                Label = PREOCUPAT_CONTROLATOR_LABEL,
                Nickname = PREOCUPAT_CONTROLATOR_NICKNAME,
                Summary = PREOCUPAT_CONTROLATOR_SUMMARY
            }
        },
        {
            MANIPULATIV_DEFENSIV_ID,
            new AttachmentStyleInfo
            {
                Id = MANIPULATIV_DEFENSIV_ID,
                Label = MANIPULATIV_DEFENSIV_LABEL,
                Nickname = MANIPULATIV_DEFENSIV_NICKNAME,
                Summary = MANIPULATIV_DEFENSIV_SUMMARY
            }
        },
        {
            ECHILIBRAT_ID,
            new AttachmentStyleInfo
            {
                Id = ECHILIBRAT_ID,
                Label = ECHILIBRAT_LABEL,
                Nickname = ECHILIBRAT_NICKNAME,
                Summary = ECHILIBRAT_SUMMARY
            }
        }
    };
            ToxicHabitsByStyle = new Dictionary<int, ToxicHabitsSection>
            {
                [1] = new ToxicHabitsSection
                {
                    Title = ANXIOS_PREOCUPAT_TITLE,
                    Habits = new List<ToxicHabit>
                    {
                        new() { Title = ANXIOS_PREOCUPAT_SUBTITLE_1, Description = ANXIOS_PREOCUPAT_DESCRIPTION_1 },
                        new() { Title = ANXIOS_PREOCUPAT_SUBTITLE_2, Description = ANXIOS_PREOCUPAT_DESCRIPTION_2 },
                        new() { Title = ANXIOS_PREOCUPAT_SUBTITLE_3, Description = ANXIOS_PREOCUPAT_DESCRIPTION_3 },
                        new() { Title = ANXIOS_PREOCUPAT_SUBTITLE_4, Description = ANXIOS_PREOCUPAT_DESCRIPTION_4 },
                    }
                },
                [2] = new ToxicHabitsSection
                {
                    Title = EVITANT_DEZANGAJAT_TITLE,
                    Habits = new List<ToxicHabit>
                    {
                        new() { Title = EVITANT_DEZANGAJAT_SUBTITLE_1, Description = EVITANT_DEZANGAJAT_DESCRIPTION_1 },
                        new() { Title = EVITANT_DEZANGAJAT_SUBTITLE_2, Description = EVITANT_DEZANGAJAT_DESCRIPTION_2 },
                        new() { Title = EVITANT_DEZANGAJAT_SUBTITLE_3, Description = EVITANT_DEZANGAJAT_DESCRIPTION_3 },
                        new() { Title = EVITANT_DEZANGAJAT_SUBTITLE_4, Description = EVITANT_DEZANGAJAT_DESCRIPTION_4 },
                    }
                },
                [3] = new ToxicHabitsSection
                {
                    Title = PREOCUPAT_CONTROLATOR_TITLE,
                    Habits = new List<ToxicHabit>
                    {
                        new() { Title = PREOCUPAT_CONTROLATOR_SUBTITLE_1, Description = PREOCUPAT_CONTROLATOR_DESCRIPTION_1 },
                        new() { Title = PREOCUPAT_CONTROLATOR_SUBTITLE_2, Description = PREOCUPAT_CONTROLATOR_DESCRIPTION_2 },
                        new() { Title = PREOCUPAT_CONTROLATOR_SUBTITLE_3, Description = PREOCUPAT_CONTROLATOR_DESCRIPTION_3 },
                        new() { Title = PREOCUPAT_CONTROLATOR_SUBTITLE_4, Description = PREOCUPAT_CONTROLATOR_DESCRIPTION_4 },
                    }
                },
                [4] = new ToxicHabitsSection
                {
                    Title = MANIPULATIV_DEFENSIV_TITLE,
                    Habits = new List<ToxicHabit>
                    {
                        new() { Title = MANIPULATIV_DEFENSIV_SUBTITLE_1, Description = MANIPULATIV_DEFENSIV_DESCRIPTION_1 },
                        new() { Title = MANIPULATIV_DEFENSIV_SUBTITLE_2, Description = MANIPULATIV_DEFENSIV_DESCRIPTION_2 },
                        new() { Title = MANIPULATIV_DEFENSIV_SUBTITLE_3, Description = MANIPULATIV_DEFENSIV_DESCRIPTION_3 },
                        new() { Title = MANIPULATIV_DEFENSIV_SUBTITLE_4, Description = MANIPULATIV_DEFENSIV_DESCRIPTION_4 },
                    }
                },
                [5] = new ToxicHabitsSection
                {
                    Title = ECHILIBRAT_TITLE,
                    Habits = new List<ToxicHabit>
                    {
                        new() { Title = ECHILIBRAT_SUBTITLE_1, Description = ECHILIBRAT_DESCRIPTION_1 },
                        new() { Title = ECHILIBRAT_SUBTITLE_2, Description = ECHILIBRAT_DESCRIPTION_2 },
                        new() { Title = ECHILIBRAT_SUBTITLE_3, Description = ECHILIBRAT_DESCRIPTION_3 },
                        new() { Title = ECHILIBRAT_SUBTITLE_4, Description = ECHILIBRAT_DESCRIPTION_4 },
                    }
                }

            };
            EmotionalNeeds = new List<EmotionalNeed>
        {
            new EmotionalNeed
            {
                Title = "Să fii înțeles(ă)",
                Description = "Să simți că partenerul te ascultă cu adevărat și îți înțelege trăirile."
            },
            new EmotionalNeed
            {
                Title = "Să te simți în siguranță",
                Description = "Să știi că poți fi tu însuți/însăți fără frică de judecată sau abandon."
            },
            new EmotionalNeed
            {
                Title = "Să fii valorizat(ă)",
                Description = "Să simți că ceea ce ești și ceea ce faci contează pentru partener."
            },
            new EmotionalNeed
            {
                Title = "Responsivitate emoțională",
                Description = "Ca partenerul să reacționeze cu empatie atunci când ai nevoie."
            },
            new EmotionalNeed
            {
                Title = "Încredere",
                Description = "Să te poți baza pe celălalt și să știi că este acolo pentru tine."
            },
            new EmotionalNeed
            {
                Title = "Sprijin în vulnerabilitate",
                Description = "Să poți fi deschis(ă) și rănit(ă) fără a fi atacat(ă) sau ignorat(ă)."
            },
            new EmotionalNeed
            {
                Title = "Afecțiune & atingere",
                Description = "Gesturi fizice care exprimă iubire și conectare (îmbrățișări, mângâieri)."
            },
            new EmotionalNeed
            {
                Title = "Autonomie & spațiu personal",
                Description = "Libertatea de a avea timp pentru tine fără vină sau presiune."
            },
            new EmotionalNeed
            {
                Title = "Claritate în comunicare",
                Description = "Dialog sincer, direct, fără ambiguitate sau „ghiceli”."
            },
            new EmotionalNeed
            {
                Title = "Scop comun",
                Description = "Să simți că mergeți în aceeași direcție, cu planuri și valori împărtășite."
            }
        };

        }


    }
}
