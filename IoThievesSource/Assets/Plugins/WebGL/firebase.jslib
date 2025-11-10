mergeInto(LibraryManager.library, {
  FB_SetTarget: function (goPtr) {
    const go = UTF8ToString(goPtr);
    if (window.FB && window.FB.setTarget) window.FB.setTarget(go);
  },
  FB_Save: function (jsonPtr) {
    const json = UTF8ToString(jsonPtr);
    if (window.FB && window.FB.save) window.FB.save(json);
  },
  FB_Load: function () {
    if (window.FB && window.FB.load) window.FB.load();
  }
});