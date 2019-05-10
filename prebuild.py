import os
import shutil

import buildcommon

if __name__ == '__main__':
    root_dir = os.path.dirname(__file__)
    assembly_version_cs = 'src/AssemblyVersion.cs'
    assembly_version_cs_bak = 'src/AssemblyVersion.cs.bak'

    shutil.copy(os.environ['ASSEMBLY_KEY_PATH'],
                os.path.join(root_dir, 'key.snk'))

    with open('version.txt', 'r') as f:
        semantic_version = f.read()

    version = semantic_version + '.' + (os.environ.get('BUILD_COUNTER') or '0')

    print("##teamcity[buildNumber '" + version + "']")
    print("##teamcity[setParameter name='system.Version' value='" + semantic_version + "']")
