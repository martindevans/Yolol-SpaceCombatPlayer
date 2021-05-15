mergeInto(LibraryManager.library, {

  Yonder_SetApplicationUrl: function(url) {
    url = Pointer_stringify(url)
    console.log("Pushing Url State: " + url);
    window.history.pushState({}, "", url);
  },
});