const actionHelper = {
    async get(url) {
        try {
            const response = await fetch(url, { method: 'GET' });
            return await this._handleResponse(response);
        } catch (error) {
            return this._handleError(error);
        }
    },

    async getWithParams(url, params = {}) {
        try {
            const queryString = this._buildQueryString(params);
            const fullUrl = queryString ? `${url}?${queryString}` : url;

            const response = await fetch(fullUrl, { method: 'GET' });
            return await this._handleResponse(response);
        } catch (error) {
            return this._handleError(error);
        }
    },

    async postJson(url, data) {
        try {
            const response = await fetch(url, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data)
            });
            return await this._handleResponse(response);
        } catch (error) {
            return this._handleError(error);
        }
    },

    async postFormData(url, formData) {
        try {
            const response = await fetch(url, {
                method: 'POST',
                body: formData // browser sets correct multipart headers
            });
            return await this._handleResponse(response);
        } catch (error) {
            return this._handleError(error);
        }
    },

    async _handleResponse(response) {
        const result = await response.json().catch(() => ({}));
        if (!response.ok) {
            return {
                success: false,
                message: result.message || "Server error",
                errors: result.errors || []
            };
        }
        return result;
        //return {
        //    success: result.isSuccess ?? result.success ?? true,
        //    message: result.message || "Operation completed",
        //    data: result.data ?? result
        //};
    },

    _handleError(error) {
        console.error("API call failed:", error);
        return { success: false, message: error.message || "Unexpected error" };
    },

    _buildQueryString(params) {
        const searchParams = new URLSearchParams();

        for (const [key, value] of Object.entries(params)) {
            if (value !== null && value !== undefined) {
                searchParams.append(key, value.toString());
            }
        }

        return searchParams.toString();
    }
};


//Method uses:
// const result = await apiHelper.get('/Contact/GetAllContacts'); // get

//const saveResponse = await apiHelper.postJson('/Contact/SaveBulkUploadContacts', validationData); // post object

//const formData = new FormData();
//formData.append('CountryId', 1);
//formData.append('LanguageId', 2);
//formData.append('ContactSource', 'Web');
//formData.append('ChannelPreference', 'Email');
//formData.append('UploadedFile', selectedFile);

//const validationResult = await apiHelper.postFormData('/Contact/GetBulkUploadValidations', formData); // post formdata


