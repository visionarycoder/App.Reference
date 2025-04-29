param name string
param location string
param logWorkspaceName string

resource ws 'Microsoft.OperationalInsights/workspaces@2021-12-01-preview' = {
  name:  logWorkspaceName
  location
  sku: { name: 'PerGB2018' }
}

resource env 'Microsoft.App/managedEnvironments@2023-05-01' = {
  name: name
  location
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId:  ws.properties.customerId
        sharedKey:   listKeys(ws.id, ws.apiVersion).primarySharedKey
      }
    }
  }
}
output envName string = env.name
