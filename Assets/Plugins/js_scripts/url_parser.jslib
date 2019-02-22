mergeInto(LibraryManager.library, {

	doesURLContainColorScheme: function() {
		// the base URL for the app is: http://digital-development.org/WormGUIDES/
		var baseURL = "http://digital-development.org/WormGUIDES/";
		if (window.location.href.includes("/WormGUIDES/")) {
			// check if there is anything after the last substring in the base URL
			if (window.location.href.length > baseURL.length) {
				// we can't know exactly what's going to be there, but we do know some properties of a valid URL that we'll check here:
				// 1. ends with /Android/
				// 2. contains /scale=
				// 3. contains /view/time=

				// first, extract everything after the base url into it's own variable
				var colorSchemeURLCandidate = window.location.substring(baseURL.length);

				if (colorSchemeURLCandidate.endsWith("/Android/") && colorSchemeURLCandidate.includes("/view/time=") && colorSchemeURLCandidate.includes("/scale=")) {
					window.alert("Found valid color scheme in URL");
					return true;
				} else {
					window.alert("Color scheme candidate in URL did not meet criteria");
					return false;
				}
			} else {
				window.alert("There is no extended URL");
			}
		} else {
			window.alert("Problems with base URL");
		}
	}, 

	extractColorSchemeFromURL: function() {
		// just make sure that the URL actually does contain a color scheme. This is normally called after first checking, but just be sure
		if (!doesURLContainColorScheme()) return "";

		// the base URL for the app is: http://digital-development.org/WormGUIDES/
		var baseURL = "http://digital-development.org/WormGUIDES/";

		return window.location.href.substring(baseURL.length);
	}
});