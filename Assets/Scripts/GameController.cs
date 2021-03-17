using System;
using System.Collections.Generic;
using System.Linq;
using PlayerSlotsUtils;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private List<PlayerSlot> _playerSlots;
    public static readonly string MAX_CREATURES_KEY = "MaxCreatures";
    public static readonly String ALLOW_MULTIPLE_CREATURES = "MultipleCreatures";
    public static int maxCreatures { get; private set; }
    public static bool allowMultipleCreatures { get; private set; }

    private Dictionary<string, Decoration> decorationDict = new Dictionary<string, Decoration>();
    private List<Decoration> decorationList = new List<Decoration>();

    private void Awake()
    {
        maxCreatures = PlayerPrefs.GetInt(MAX_CREATURES_KEY);
        allowMultipleCreatures = PlayerPrefs.GetInt(ALLOW_MULTIPLE_CREATURES) != 0;
        EventsManager.onTwitchCommandReceived += CommandReceived;
        EventsManager.onCreatureStartParty += CreatureStartParty;
        _playerSlots = FindObjectsOfType<PlayerSlot>().ToList();
    }

    private void OnDestroy()
    {
        EventsManager.onTwitchCommandReceived -= CommandReceived;
        EventsManager.onCreatureStartParty -= CreatureStartParty;
    }

    private void CommandReceived(TwitchCommand twitchcommand)
    {
        PlayerSlot userPS;
        switch (twitchcommand.argument)
        {
            case CommandArg.JOIN:
                if (_playerSlots.GetEmptySlot(out userPS))
                {
                    if (_playerSlots.GetPlayerSlotByName(twitchcommand.username)) return;
                    userPS.userJoin(twitchcommand.username);
                }

                break;
            case CommandArg.LEAVE:
                if (_playerSlots.GetPlayerSlotByName(out userPS, twitchcommand.username))
                {
                    userPS.UserLeave();
                }

                break;
            case CommandArg.BODYCOLOR:
            case CommandArg.BODY:
                if (_playerSlots.GetPlayerSlotByName(out userPS, twitchcommand.username))
                {
                    if (!string.IsNullOrEmpty(twitchcommand.message))
                    {
                        if (Int32.TryParse(twitchcommand.message, out int bodyNumber))
                        {
                            if (bodyNumber > 0 && bodyNumber < Enum.GetValues(typeof(BodyEnum)).Length)
                            {
                                BodyEnum bodyEnum = (BodyEnum) bodyNumber;
                                userPS.SetBody(bodyEnum);
                            }
                        }

                        if (customColors.TryGetValue(twitchcommand.message.ToUpper(), out string hexColor))
                        {
                            twitchcommand.message = hexColor;
                        }

                        if (ColorUtility.TryParseHtmlString(twitchcommand.message, out Color pcol))
                        {
                            userPS.SetPrimaryColor(pcol);
                        }
                    }
                }

                break;
            case CommandArg.PARTCOLOR:
            case CommandArg.TOP:
            case CommandArg.FRONT:
            case CommandArg.LEFT:
            case CommandArg.RIGHT:
            case CommandArg.BOTTOM:
                if (_playerSlots.GetPlayerSlotByName(out userPS, twitchcommand.username))
                {
                    if (!string.IsNullOrEmpty(twitchcommand.message))
                    {
                        if (Int32.TryParse(twitchcommand.message, out int partNumber))
                        {
                            if (partNumber >= 0 && partNumber < Enum.GetValues(typeof(PartEnum)).Length)
                            {
                                PartEnum partEnum = (PartEnum) partNumber;
                                userPS.SetPart(partEnum, twitchcommand.argument);
                            }
                        }

                        if (customColors.TryGetValue(twitchcommand.message.ToUpper(), out string hexColor))
                        {
                            twitchcommand.message = hexColor;
                        }

                        if (ColorUtility.TryParseHtmlString(twitchcommand.message, out Color scol))
                        {
                            userPS.SetSecondaryColor(scol);
                        }
                    }
                }

                break;
            case CommandArg.COMMIT:
            case CommandArg.READY:
                if (_playerSlots.GetPlayerSlotByName(out userPS, twitchcommand.username))
                {
                    userPS.UserReady();
                }

                break;
            case CommandArg.RANDOMIZE:
                if (_playerSlots.GetPlayerSlotByName(out userPS, twitchcommand.username))
                {
                    userPS.Randomize();
                }

                break;
            default:
                break;
        }
    }

    private void CreatureStartParty(Decoration decoration, string author)
    {
        if (allowMultipleCreatures)
        {
            if (decorationList.Count > maxCreatures)
            {
                Destroy(decorationList[0].gameObject);
                decorationList.Remove(decorationList[0]);
            }

            decorationList.Add(decoration);
        }
        else
        {
            if (decorationDict.ContainsKey(author))
            {
                decorationDict.TryGetValue(author, out Decoration dec);
                Destroy(dec.gameObject);

                //que me perdone dios por no saber usar bien los diccionarios.
                decorationDict.Remove(author);
            }
            else if (decorationDict.Count > maxCreatures)
            {
                KeyValuePair<string, Decoration> d = decorationDict.ElementAt(0);
                Destroy(d.Value.gameObject);
                decorationDict.Remove(d.Key);
            }

            decorationDict.Add(author, decoration);
        }
    }

    public static Dictionary<string, string> customColors = new Dictionary<string, string>
    {
        {"FJPORCEL", "#575334"},
        {"TOTALTRIDENT", "#FAE500"},
        {"SLANTAR", "#794BC4"},
        {"CHARLOTTE", "#00B5E2"},
        {"FLIPCODERS", "#75AD0A"},
        {"KATIA", "#FFDCE9"},
        {"ARY", "#FFA9A9"},
        {"NEBULOSAPASTEL", "#C8A2C8"},
        {"ZELDA", "#C8A2C8"},
        {"GRINCHVOMIT", "#115740"},
        {"GUACAMOLE", "#64BA54"},
        {"KK", "#AD7100"},
        {"ZUMITO", "#e18b00"},
        {"TORONJA", "#fa3c5a"},
        {"COLDBLUE", "#1ea896"},
        {"LIGHTCORAL", "#F08080"},
        {"SALMON", "#FA8072"},
        {"INDIANRED", "#CD5C5C"},
        {"RED", "#FF0000"},
        {"CRIMSON", "#DC143C"},
        {"FIREBRICK", "#B22222"},
        {"BROWN", "#A52A2A"},
        {"DARKRED", "#8B0000"},
        {"MAROON", "#800000"},
        {"MISTYROSE", "#FFE4E1"},
        {"PINK", "#FFC0CB"},
        {"LIGHTPINK", "#FFB6C1"},
        {"HOTPINK", "#FF69B4"},
        {"ROSYBROWN", "#BC8F8F"},
        {"PALEVIOLETRED", "#DB7093"},
        {"DEEPPINK", "#FF1493"},
        {"MEDIUMVIOLETRED", "#C71585"},
        {"PAPAYAWHIP", "#FFEFD5"},
        {"BLANCHEDALMOND", "#FFEBCD"},
        {"BISQUE", "#FFE4C4"},
        {"MOCCASIN", "#FFE4B5"},
        {"PEACHPUFF", "#FFDAB9"},
        {"NAVAJOWHITE", "#FFDEAD"},
        {"LIGHTSALMON", "#FFA07A"},
        {"DARKSALMON", "#E9967A"},
        {"ORANGE", "#FFA500"},
        {"DARKORANGE", "#FF8C00"},
        {"CORAL", "#FF7F50"},
        {"TOMATO", "#FF6347"},
        {"ORANGERED", "#FF4500"},
        {"WHEAT", "#F5DEB3"},
        {"BURLYWOOD", "#DEB887"},
        {"TAN", "#D2B48C"},
        {"SANDYBROWN", "#F4A460"},
        {"GOLDENROD", "#DAA520"},
        {"PERU", "#CD853F"},
        {"DARKGOLDENROD", "#B8860B"},
        {"CHOCOLATE", "#D2691E"},
        {"SIENNA", "#A0522D"},
        {"SADDLEBROWN", "#8B4513"},
        {"LIGHTYELLOW", "#FFFFE0"},
        {"CORNSILK", "#FFF8DC"},
        {"LEMONCHIFFON", "#FFFACD"},
        {"LIGHTGOLDENRODYELLOW", "#FAFAD2"},
        {"PALEGOLDENROD", "#EEE8AA"},
        {"KHAKI", "#F0E68C"},
        {"YELLOW", "#FFFF00"},
        {"GOLD", "#FFD700"},
        {"DARKKHAKI", "#BDB76B"},
        {"GREENYELLOW", "#ADFF2F"},
        {"CHARTREUSE", "#7FFF00"},
        {"LAWNGREEN", "#7CFC00"},
        {"YELLOWGREEN", "#9ACD32"},
        {"OLIVEDRAB", "#6B8E23"},
        {"OLIVE", "#808000"},
        {"DARKOLIVEGREEN", "#556B2F"},
        {"PALEGREEN", "#98FB98"},
        {"LIGHTGREEN", "#90EE90"},
        {"MEDIUMSPRINGGREEN", "#00FA9A"},
        {"SPRINGGREEN", "#00FF7F"},
        {"LIME", "#00FF00"},
        {"DARKSEAGREEN", "#8FBC8F"},
        {"LIMEGREEN", "#32CD32"},
        {"MEDIUMSEAGREEN", "#3CB371"},
        {"SEAGREEN", "#2E8B57"},
        {"FORESTGREEN", "#228B22"},
        {"GREEN", "#008000"},
        {"DARKGREEN", "#006400"},
        {"LIGHTCYAN", "#E0FFFF"},
        {"PALETURQUOISE", "#AFEEEE"},
        {"AQUAMARINE", "#7FFFD4"},
        {"AQUA / CYAN", "#00FFFF"},
        {"TURQUOISE", "#40E0D0"},
        {"MEDIUMTURQUOISE", "#48D1CC"},
        {"DARKTURQUOISE", "#00CED1"},
        {"MEDIUMAQUAMARINE", "#66CDAA"},
        {"LIGHTSEAGREEN", "#20B2AA"},
        {"CADETBLUE", "#5F9EA0"},
        {"DARKCYAN", "#008B8B"},
        {"TEAL", "#008080"},
        {"LAVENDER", "#E6E6FA"},
        {"BLUEWEB", "#CEE7FF"},
        {"POWDERBLUE", "#B0E0E6"},
        {"LIGHTBLUE", "#ADD8E6"},
        {"LIGHTSKYBLUE", "#87CEFA"},
        {"SKYBLUE", "#87CEEB"},
        {"LIGHTSTEELBLUE", "#B0C4DE"},
        {"DEEPSKYBLUE", "#00BFFF"},
        {"CORNFLOWERBLUE", "#6495ED"},
        {"DODGERBLUE", "#1E90FF"},
        {"STEELBLUE", "#4682B4"},
        {"ROYALBLUE", "#4169e1"},
        {"BLUE", "#0000FF"},
        {"MEDIUMBLUE", "#0000CD"},
        {"DARKBLUE", "#00008B"},
        {"NAVY", "#000080"},
        {"MIDNIGHTBLUE", "#191970"},
        {"THISTLE", "#D8BFD8"},
        {"PLUM", "#DDA0DD"},
        {"VIOLET", "#EE82EE"},
        {"ORCHID", "#DA70D6"},
        {"FUCHSIA / MAGENTA", "#FF00FF"},
        {"MEDIUMPURPLE", "#9370DB"},
        {"MEDIUMORCHID", "#BA55D3"},
        {"MEDIUMSLATEBLUE", "#7B68EE"},
        {"SLATEBLUE", "#6A5ACD"},
        {"BLUEVIOLET", "#8A2BE2"},
        {"DARKVIOLET", "#9400D3"},
        {"DARKORCHID", "#9932CC"},
        {"DARKMAGENTA", "#8B008B"},
        {"PURPLE", "#800080"},
        {"DARKSLATEBLUE", "#483D8B"},
        {"INDIGO", "#4B0082"},
        {"WHITE", "#FFFFFF"},
        {"WHITESMOKE", "#F5F5F5"},
        {"SNOW", "#FFFAFA"},
        {"SEASHELL", "#FFF5EE"},
        {"LINEN", "#FAF0E6"},
        {"ANTIQUEWHITE", "#FAEBD7"},
        {"OLDLACE", "#FDF5E6"},
        {"FLORALWHITE", "#FFFAF0"},
        {"IVORY", "#FFFFF0"},
        {"BEIGE", "#F5F5DC"},
        {"HONEYDEW", "#F0FFF0"},
        {"MINTCREAM", "#F5FFFA"},
        {"AZURE", "#F0FFFF"},
        {"ALICEBLUE", "#F0F8FF"},
        {"GHOSTWHITE", "#F8F8FF"},
        {"LAVENDERBLUSH", "#FFF0F5"},
        {"GAINSBORO", "#DCDCDC"},
        {"LIGHTGREY", "#D3D3D3"},
        {"SILVER", "#C0C0C0"},
        {"DARKGRAY", "#A9A9A9"},
        {"LIGHTSLATEGRAY", "#778899"},
        {"SLATEGRAY", "#708090"},
        {"GRAY", "#808080"},
        {"DIMGRAY", "#696969"},
        {"DARKSLATEGRAY", "#2F4F4F"},
        {"DEMIGIANT", "#E13F7E"},
        {"ROTHIO", "#E7E74E8"}
    };
}