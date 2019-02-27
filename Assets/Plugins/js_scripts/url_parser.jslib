mergeInto(LibraryManager.library, {

	DoesURLContainColorScheme: function() {
		// the base URL for the app is: http://digital-development.org/WormGUIDES/
		var baseURL = "http://digital-development.org/WormGUIDES/";

		if (window.location.href.includes(baseURL)) {
			// check if there is anything after the last substring in the base URL
			if (window.location.href.length > baseURL.length) {
				// we can't know exactly what's going to be there, but we do know some properties of a valid URL that we'll check here:
				// 1. ends with /Android/
				// 2. contains /scale=
				// 3. contains /view/time=

				// first, extract everything after the base url into it's own variable
				var colorSchemeURLCandidate = window.location.href.substring(baseURL.length);

				if (colorSchemeURLCandidate.endsWith("/Android/") && colorSchemeURLCandidate.includes("/view/time=") && colorSchemeURLCandidate.includes("/scale=")) {
					//window.alert("Found valid color scheme in URL: " + colorSchemeURLCandidate);
					return true;
				}
			}
		}

		return false;
	}, 

	ExtractColorSchemeFromURL: function() {
		// the base URL for the app is: http://digital-development.org/WormGUIDES/
		var baseURL = "http://digital-development.org/WormGUIDES/";

		// extract the color scheme as a javascript string
		var colorSchemeStr = window.location.href.substring(baseURL.length);
		
		// strings required some work to return to c#. Adapted from: https://docs.unity3d.com/Manual/webgl-interactingwithbrowserscripting.html
		var bufferSize = lengthBytesUTF8(colorSchemeStr) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(colorSchemeStr, buffer, bufferSize);

		return buffer;
	}
});