import * as dotenv from 'dotenv';
import { resolve } from 'path';
dotenv.config({ path: resolve(__dirname, '../.env') });

import { NestFactory } from '@nestjs/core';
import { AppModule } from './app.module';
import {DocumentBuilder, SwaggerModule} from "@nestjs/swagger";
import {OAuthFlowsObject} from "@nestjs/swagger/dist/interfaces/open-api-spec.interface";

async function bootstrap() {
  const app = await NestFactory.create(AppModule, { cors: true });
  app.setGlobalPrefix('/api');
  //        var scheme = new OpenApiSecurityScheme
    //         {
    //             Name = HeaderNames.Authorization,
    //             Type = SecuritySchemeType.OAuth2,
    //             Scheme = JwtBearerDefaults.AuthenticationScheme,
    //             BearerFormat = JwtConstants.TokenType,
    //             In = ParameterLocation.Header,
    //             
    //             Flows = new OpenApiOAuthFlows
    //             {
    //                 Password = new OpenApiOAuthFlow
    //                 {
    //                     TokenUrl = new Uri("https://localhost:5021/auth/connect/token")
    //                 }
    //             }
    //         };
  const config = new DocumentBuilder()
      .setTitle("LiWiMus.Admin.Web.API.Nest")
      .addServer("http://localhost:3001")
      .addOAuth2({
          type: 'oauth2',
          flows: {
              password: {
                  tokenUrl: 'http://localhost:5020/auth/connect/token',
                  scopes: []
              }
          }

      }, 'swagger')
      .build();

  const document = SwaggerModule.createDocument(app, config);
  
  SwaggerModule.setup('swagger', app, document);
  
  await app.listen(3001);
}
bootstrap();