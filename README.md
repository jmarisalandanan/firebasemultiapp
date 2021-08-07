# Firebase Unity multi app/project/config support

### Summary:
For instances where you would choose to use multiple firebase projects(example one for dev and prod) in your unity project this preprocessor will help you automate
the otherwise manual and prone to mistake process. This script exchanges firebase config files depending on a define you set. This tools is only tested and supports mobile platforms (iOS and Android) but should be easy to extend with other platforms as well.

### Usage
1. Import this [package](https://github.com/jmarisalandanan/firebasemultiapp/releases/tag/1)  to your project
2. Add your firebase configs for both platforms in the directory declared in the script. In this sample project's case it's under the root directory `/multiapp/development` or `/multiapp/release`
3. Add the scripting define on your build settings. If you want to build with the config used in `/multiapp/release`, Add the `FIREBASE_PROD` define, else remove the define.
4. Build and observe that the correct firebase config is used
