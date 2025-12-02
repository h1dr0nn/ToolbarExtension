# Configuration

## Play From Loading Scene

The "Play From Loading" toolbar button requires configuration to work properly.

Set your loading scene path in Unity:

```csharp
// Add this to any Editor script to set your loading scene path
EditorPrefs.SetString("h1dr0n_loading_scene_path", "Assets/YourPath/LoadingScene.unity");
```

Or via Unity's menu: `Edit → Preferences → External Tools`
