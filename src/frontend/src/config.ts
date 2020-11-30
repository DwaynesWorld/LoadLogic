interface AppConfiguration {
  endpoints: {
    customers_baseurl: string;
    locations_baseurl: string;
  };
}

const local: AppConfiguration = {
  endpoints: {
    customers_baseurl: "http://localhost:30000/c/v1",
    locations_baseurl: "http://localhost:30000/l/v1",
  },
};

export enum Environment {
  Local,
  Dev,
  Staging,
  Production,
}

const getCurrentEnvironment = () => {
  const hostname = window.location.hostname.toLowerCase();

  if (hostname.includes("localhost")) return Environment.Local;
  // TODO: Setup environments
  return Environment.Production;
};

export const getEnvironmentConfig = () => {
  const currentEnvironment = getCurrentEnvironment();

  switch (currentEnvironment) {
    case Environment.Local:
      return local;
    default:
      return local;
  }
};

export default {
  ...getEnvironmentConfig(),
};
