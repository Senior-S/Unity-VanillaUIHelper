using System.IO;
using SDG.Unturned;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace SeniorS.UIHelper
{
    public class VanillaUIHelper : EditorWindow
    {
        private const string darkThemePackageLink = "https://github.com/DanielWillett/UnturnedUIAssets/raw/refs/heads/main/UI/uGUI/DarkTheme/uGUI%20Assets.unitypackage";

        [MenuItem("Tools/Vanilla UI Helper")]
        public static void OpenWindow()
        {
            GetWindow<VanillaUIHelper>("Vanilla UI Helper");
        }

        public void OnEnable()
        {
            if (!Directory.Exists("Assets/BlazingFlame/uGUI/Prefabs"))
            {
                EditorUtility.DisplayDialog("Vanilla UI Helper", "This tool requires the DarkTheme unitypackage from Daniel Willett (BlazingFlame) you can download it manually clicking on 'DarkTheme package link' or using the button above it.", "OK");
                return;
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("Vanilla UI Helper", EditorStyles.boldLabel);
            GUILayout.Label("by SeniorS", EditorStyles.miniLabel);
            EditorGUILayout.Space(8);

            GUILayout.Label("Essential", EditorStyles.boldLabel);
            EditorGUILayout.Space(2);
            if (GUILayout.Button("Setup Canvas"))
            {
                SetupCanvas();
            }
            if (GUILayout.Button("Copy path to object"))
            {
                CopyPathToObject();
            }
            if (GUILayout.Button("Download DarkTheme UnityPackage"))
            {
                DownloadUnityPackage();
            }
            if (GUILayout.Button("DarkTheme package link"))
            {
                Application.OpenURL("https://github.com/DanielWillett/UnturnedUIAssets/blob/main/UI/uGUI/DarkTheme/uGUI%20Assets.unitypackage");
            }

            if (GUILayout.Button("Check Unturned UI Assets"))
            {
                Application.OpenURL("https://github.com/DanielWillett/UnturnedUIAssets/tree/main");
            }
            EditorGUILayout.Space(2);

            GUILayout.Label("Button", EditorStyles.label);
            EditorGUILayout.Space(2);
            if (GUILayout.Button("Default Button"))
            {
                AddPrefab("Button/uGUI Button");
            }
            if (GUILayout.Button("Disableable Button"))
            {
                AddPrefab("Button/uGUI Disableable Button");
            }
            if (GUILayout.Button("Right Clickable Button"))
            {
                AddPrefab("Button/uGUI RightClickable Button");
            }
            if (GUILayout.Button("Disableable Right Clickable Button"))
            {
                AddPrefab("Button/uGUI Disableable RightClickable Button");
            }

            EditorGUILayout.Space(6);
            GUILayout.Label("Input Field", EditorStyles.label);
            EditorGUILayout.Space(2);
            if (GUILayout.Button("Default Input Field"))
            {
                AddPrefab("Input Field/uGUI Input Field");
            }
            if (GUILayout.Button("Disableable Input Field"))
            {
                AddPrefab("Input Field/uGUI Disableable Input Field");
            }

            EditorGUILayout.Space(6);
            GUILayout.Label("Label", EditorStyles.label);
            EditorGUILayout.Space(2);
            if (GUILayout.Button("Default Label"))
            {
                AddPrefab("Label/uGUI Default Label");
            }
            if (GUILayout.Button("Outline Label"))
            {
                AddPrefab("Label/uGUI Outline Label");
            }
            if (GUILayout.Button("Shadow Label"))
            {
                AddPrefab("Label/uGUI Shadow Label");
            }
            if (GUILayout.Button("Tooltip"))
            {
                AddPrefab("Label/uGUI Tooltip");
            }

            EditorGUILayout.Space(6);
            GUILayout.Label("ScrolView", EditorStyles.label);
            EditorGUILayout.Space(2);
            if (GUILayout.Button("Default ScrollView"))
            {
                AddPrefab("ScrollView/uGUI ScrollView");
            }
            if (GUILayout.Button("Alternative ScrollView"))
            {
                AddScrollViewCode();
            }
            if (GUILayout.Button("ScrollView (Vertical)"))
            {
                AddPrefab("ScrollView/uGUI ScrollView (Vertical)");
            }
            if (GUILayout.Button("ScrollView (Horizontal)"))
            {
                AddPrefab("ScrollView/uGUI ScrollView (Horizontal)");
            }
            if (GUILayout.Button("Disableable ScrollView"))
            {
                AddPrefab("ScrollView/uGUI Disableable ScrollView");
            }

            EditorGUILayout.Space(6);
            GUILayout.Label("Toggle", EditorStyles.label);
            EditorGUILayout.Space(2);
            if (GUILayout.Button("Default Toggle"))
            {
                AddPrefab("Toggle/uGUI Toggle");
            }
            if (GUILayout.Button("Alternative Toggle"))
            {
                AddToggleCode();
            }
            if (GUILayout.Button("Plugin-Controlled Toggle"))
            {
                AddPrefab("Toggle/uGUI Plugin-Controlled Toggle");
            }
            if (GUILayout.Button("Disableable Toggle"))
            {
                AddPrefab("Toggle/uGUI Disableable Toggle");
            }

            EditorGUILayout.Space(6);
            GUILayout.Label("Box", EditorStyles.label);
            EditorGUILayout.Space(2);
            if (GUILayout.Button("Default Box"))
            {
                AddPrefab("Box/uGUI Box");
            }
            if (GUILayout.Button("Shadow Box Label"))
            {
                AddPrefab("Box/uGUI Box Shadow Label");
            }
            if (GUILayout.Button("Outline Box Label"))
            {
                AddPrefab("Box/uGUI Box Outline Label");
            }
            if (GUILayout.Button("Default Box Label"))
            {
                AddPrefab("Box/uGUI Box Default Label");
            }

            EditorGUILayout.Space(10);
            GUILayout.Label("Version: 1.0.2", EditorStyles.boldLabel);
        }

        private void CopyPathToObject()
        {
            if (!CheckSelection()) return;
            Transform currentTransform = Selection.activeTransform;
            string path = currentTransform.name;
            while (currentTransform.parent != null)
            {
                currentTransform = currentTransform.parent;
                path = currentTransform.name + "/" + path;
                if(currentTransform.name == "Canvas") break;
            }

            EditorGUIUtility.systemCopyBuffer = path;
            ShowNotification(new GUIContent("GameObject path copied to clipboard!"));
        }

        private void DownloadUnityPackage()
        {
            if (Directory.Exists("Assets/BlazingFlame/uGUI/Prefabs"))
            {
                EditorUtility.DisplayDialog("Vanilla UI Helper", "The DarkTheme Unity package is already installed.", "OK");
                return;
            }

            EditorUtility.DisplayProgressBar("Downloading", "Downloading the Unturned DarkTheme Unity package...", 0);
            UnityWebRequest request = UnityWebRequest.Get(darkThemePackageLink);

            request.SendWebRequest().completed += operation =>
            {
                EditorUtility.ClearProgressBar();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string path = "Assets/uGUI Assets.unitypackage";
                    File.WriteAllBytes(path, request.downloadHandler.data);

                    AssetDatabase.ImportPackage(path, true);

                    Debug.Log("Package downloaded and imported successfully.");
                }
                else
                {
                    Debug.LogError($"Failed to download the package: {request.error}");
                }
            };
        }

        private void SetupCanvas()
        {
            if (!CheckSelection()) return;
            GameObject effectGameObject = Selection.activeGameObject;
            if (!effectGameObject.name.Equals("Effect"))
            {
                EditorUtility.DisplayDialog("Vanilla UI Helper", "Canvas must only be added to gameobject called 'Effect'.", "OK");
                return;
            }
            GameObject canvasGameObject = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGameObject.transform.SetParent(effectGameObject.transform);
            RectTransform rectTransform = canvasGameObject.GetComponent<RectTransform>();
            SetupRectTransform(rectTransform);

            Canvas canvas = canvasGameObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 0;
            canvas.pixelPerfect = true;
            canvas.targetDisplay = 0;
            canvas.additionalShaderChannels = (AdditionalCanvasShaderChannels)011001; // TextCoord1, Normal, Tangent

            CanvasScaler scaler = canvasGameObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            scaler.scaleFactor = 1f;
            scaler.referencePixelsPerUnit = 25;

            UnturnedCanvasScaler unturnedCanvasScaler = canvasGameObject.AddComponent<UnturnedCanvasScaler>();
            unturnedCanvasScaler.scaler = scaler;

            Selection.activeTransform = canvasGameObject.transform;
        }

        private void AddPrefab(string path)
        {
            if (!CheckSelection()) return;
            GameObject buttonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/BlazingFlame/uGUI/Prefabs/{path}.prefab");
            GameObject buttonGameObject = Instantiate(buttonPrefab);
            buttonGameObject.name = buttonGameObject.name.Remove(buttonGameObject.name.Length - 7);
            buttonGameObject.transform.SetParent(Selection.activeTransform);
            RectTransform rectTransform = buttonGameObject.GetComponent<RectTransform>();
            SetupRectTransform(rectTransform);

            Selection.activeTransform = buttonGameObject.transform;
        }

        private void AddToggleCode()
        {
            if (!CheckSelection()) return;

            GameObject buttonGameObject = new GameObject("Toggle", typeof(RectTransform), typeof(Image), typeof(Button));
            buttonGameObject.layer = 5;
            buttonGameObject.transform.SetParent(Selection.activeTransform);
            RectTransform rectTransform = buttonGameObject.GetComponent<RectTransform>();
            SetupRectTransform(rectTransform, 30, 30);
            Image image = buttonGameObject.GetComponent<Image>();
            image.color = new Color(0.9f, 0.9f, 0.9f);
            image.type = Image.Type.Sliced;
            image.pixelsPerUnitMultiplier = 0.15f;
            image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/BlazingFlame/uGUI/Box.png");
            Button button = buttonGameObject.GetComponent<Button>();
            button.transition = Selectable.Transition.SpriteSwap;
            button.spriteState = new SpriteState
            {
                highlightedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/BlazingFlame/uGUI/Box_Highlighted.png"),
                pressedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/BlazingFlame/uGUI/Box_Pressed.png")
            };

            GameObject checkGameobject = new GameObject("CheckIcon", typeof(RectTransform), typeof(Image));
            checkGameobject.layer = 5;
            checkGameobject.transform.SetParent(buttonGameObject.transform);
            RectTransform checkRectTransform = checkGameobject.GetComponent<RectTransform>();
            SetupRectTransform(checkRectTransform, 50, 50);
            checkRectTransform.anchoredPosition = new Vector2(0, -1);
            image = checkGameobject.GetComponent<Image>();
            image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/BlazingFlame/uGUI/Toggle_Foreground.png");
            image.color = Color.white;
            checkGameobject.SetActive(false);

            Selection.activeTransform = buttonGameObject.transform;
        }

        private void AddScrollViewCode()
        {
            if (!CheckSelection()) return;

            GameObject scrollViewGameObject = new GameObject("ScrollView", typeof(RectTransform), typeof(Image), typeof(ScrollRect));
            scrollViewGameObject.layer = 5;
            scrollViewGameObject.transform.SetParent(Selection.activeTransform);
            RectTransform rectTransform = scrollViewGameObject.GetComponent<RectTransform>();
            SetupRectTransform(rectTransform, 200, 200);
            Image image = scrollViewGameObject.GetComponent<Image>();
            image.color = new Color(0f, 0f, 0f, 0f);
            image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/BlazingFlame/uGUI/Box.png");
            ScrollRect scrollRect = scrollViewGameObject.GetComponent<ScrollRect>();
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.scrollSensitivity = 5;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
            scrollRect.horizontal = false;

            GameObject viewportGameObject = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(Mask));
            viewportGameObject.layer = 5;
            viewportGameObject.transform.SetParent(scrollViewGameObject.transform);
            RectTransform viewportRectTransform = viewportGameObject.GetComponent<RectTransform>();
            SetupRectTransform(viewportRectTransform);
            StrecthRectTransform(viewportRectTransform);
            Image viewportImage = viewportGameObject.GetComponent<Image>();
            viewportImage.color = new Color(0.9f, 0.9f, 0.9f, 0.4f);
            viewportImage.type = Image.Type.Sliced;
            viewportImage.pixelsPerUnitMultiplier = 0.8f;
            viewportImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/BlazingFlame/uGUI/Box.png");

            GameObject contentGameObject = new GameObject("Content", typeof(RectTransform), typeof(Image));
            contentGameObject.layer = 5;
            contentGameObject.transform.SetParent(viewportGameObject.transform);

            RectTransform contentRectTransform = contentGameObject.GetComponent<RectTransform>();
            scrollRect.content = contentRectTransform;
            SetupRectTransform(contentRectTransform, 0, 200);
            contentRectTransform.anchorMax = new Vector2(1, 1);
            contentRectTransform.anchorMin = new Vector2(0, 1);
            Image contentImage = contentGameObject.GetComponent<Image>();
            contentImage.color = new Color(0, 0, 0, 0f);
            contentImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/BlazingFlame/uGUI/Box.png");

            GameObject verticalScrollGameObject = new GameObject("VerticalScrollbar", typeof(RectTransform), typeof(Image), typeof(Scrollbar));
            verticalScrollGameObject.layer = 5;
            verticalScrollGameObject.transform.SetParent(scrollViewGameObject.transform);
            RectTransform verticalScrollRectTransform = verticalScrollGameObject.GetComponent<RectTransform>();
            SetupRectTransform(verticalScrollRectTransform, 0, 0);
            verticalScrollRectTransform.pivot = new Vector2(1, 1);
            verticalScrollRectTransform.anchorMax = new Vector2(1, 1);
            verticalScrollRectTransform.anchorMin = new Vector2(1, 0);
            verticalScrollRectTransform.anchoredPosition = new Vector2(10, -5f);
            verticalScrollRectTransform.sizeDelta = new Vector2(3, -10f);

            Image verticalScrollImage = verticalScrollGameObject.GetComponent<Image>();
            verticalScrollImage.color = new Color(0.9f, 0.9f, 0.9f);
            verticalScrollImage.type = Image.Type.Sliced;
            verticalScrollImage.pixelsPerUnitMultiplier = 1f;
            verticalScrollImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/BlazingFlame/uGUI/Slider_Background.png");

            GameObject verticalSlidingAreaGameObject = new GameObject("Sliding Area", typeof(RectTransform));
            verticalSlidingAreaGameObject.layer = 5;
            verticalSlidingAreaGameObject.transform.SetParent(verticalScrollGameObject.transform);
            RectTransform verticalSlidingAreaRectTransform = verticalSlidingAreaGameObject.GetComponent<RectTransform>();
            SetupRectTransform(verticalSlidingAreaRectTransform, 3, 10);
            StrecthRectTransform(verticalSlidingAreaRectTransform);
            verticalSlidingAreaRectTransform.anchoredPosition = new Vector2(0, 0);
            verticalSlidingAreaRectTransform.sizeDelta = new Vector2(0, -10f);

            GameObject verticalHandleGameObject = new GameObject("Handle", typeof(RectTransform), typeof(Image));
            verticalHandleGameObject.layer = 5;
            verticalHandleGameObject.transform.SetParent(verticalSlidingAreaGameObject.transform);
            RectTransform verticalHandleRectTransform = verticalHandleGameObject.GetComponent<RectTransform>();
            SetupRectTransform(verticalHandleRectTransform, 20, 20);
            StrecthRectTransform(verticalHandleRectTransform);
            verticalHandleRectTransform.sizeDelta = new Vector2(8, 20);
            
            Image verticalHandleImage = verticalHandleGameObject.GetComponent<Image>();
            verticalHandleImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/BlazingFlame/uGUI/Box.png");
            verticalHandleImage.color = new Color(0.9f, 0.9f, 0.9f, 0.9f);
            verticalHandleImage.type = Image.Type.Sliced;
            verticalHandleImage.pixelsPerUnitMultiplier = 0.4f;

            Scrollbar verticalScrollbar = verticalScrollGameObject.GetComponent<Scrollbar>();
            verticalScrollbar.direction = Scrollbar.Direction.BottomToTop;
            verticalScrollbar.handleRect = verticalHandleRectTransform;

            scrollRect.verticalScrollbar = verticalScrollbar;
            scrollRect.viewport = viewportRectTransform;

            Selection.activeTransform = scrollViewGameObject.transform;
        }

        private void SetupRectTransform(RectTransform rectTransform, float x = 20f, float y = 20f)
        {
            rectTransform.localScale = Vector3.one;
            rectTransform.sizeDelta = new Vector2(x, y);
            rectTransform.anchoredPosition = Vector2.zero;
        }

        private void SetupRectTransform(RectTransform rectTransform)
        {
            rectTransform.localScale = Vector3.one;
            rectTransform.anchoredPosition = Vector2.zero;
        }

        private void StrecthRectTransform(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.sizeDelta = new Vector2(0, 0);
        }

        private bool CheckSelection()
        {
            if(Selection.activeTransform == null)
            {
                EditorUtility.DisplayDialog("Vanilla UI Helper", "Please select a gameobject to add UI elements to.", "OK");
                return false;
            }

            return true;
        }
    }
}
