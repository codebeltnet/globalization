$version = minver -i -t v -v w
docker tag globalization-docfx:$version jcr.codebelt.net/geekle/globalization-docfx:$version
docker push jcr.codebelt.net/geekle/globalization-docfx:$version
