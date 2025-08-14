const BASE_URL = "http://localhost:5153";

export const apiFetch = async (endpoint, options = {}) => {
  const token = localStorage.getItem("token");

  const defaultHeaders = {
    "Content-Type": "application/json",
    "X-API-Version": "1.0",
    ...(token ? { "Authorization": `Bearer ${token}` } : {})
  };

  const config = {
    method: options.method || "GET",
    headers: {
      ...defaultHeaders,
      ...options.headers,
    },
    ...(options.body && { body: JSON.stringify(options.body) }),
  };

  const response = await fetch(`${BASE_URL}${endpoint}`, config);

  const contentType = response.headers.get("Content-Type");
  let data = null;

  if (contentType && contentType.includes("application/json")) {
    data = await response.json();
  } else {
    data = await response.text();
  }

  return {
    status: response.status,
    ok: response.ok,
    data,
  };
};
