export const environment = {
  production: false,
  apiUrl: 'http://localhost:5263'
};
  
export const deployed_environment = {
  production: true,
  apiUrl: 'https://proiect-net-2024-production.up.railway.app'
};

  // Dynamic environment selector
export const currentEnvironment = (() => {
  // Detect if running on Vercel by checking the hostname
  if (typeof window !== 'undefined' && window.location.hostname.includes('vercel.app')) {
      return deployed_environment;
  }
  // Fallback to local development environment
  return environment;
})();
  