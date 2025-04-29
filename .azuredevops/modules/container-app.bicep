param location string
param appName  string
param envName  string
param image    string
param registryLoginServer  string
param registryUsername     string
param registryPassword     string

resource app 'Microsoft.App/containerApps@2023-05-01' = {
  name: appName
  location
  identity: { type: 'SystemAssigned' }
  properties: {
    managedEnvironmentId: resourceId('Microsoft.App/managedEnvironments', envName)
    configuration: {
      registries: [
        {
          server:   registryLoginServer
          username: registryUsername
          passwordSecretRef: 'acrPwd'
        }
      ]
      secrets: [{ name: 'acrPwd', value: registryPassword }]
    }
    template: {
      containers: [{
        name: 'api'
        image: image
        env: [
          { name: 'ConnectionStrings__Sql', value: reference(resourceId('Microsoft.KeyVault/vaults/secrets', '${appName}kv', 'Sql'), '2022-07-01-preview').value }
        ]
        resources: { cpu: 0.5, memory: '1Gi' }
      }]
      ingress: { external: true, targetPort: 8080 }
      scale: { minReplicas: 1, maxReplicas: 3 }
    }
  }
}
output url string = 'https://${app.properties.configuration.ingress.fqdn}'
