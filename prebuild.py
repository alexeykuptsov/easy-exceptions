import os
import shutil

import buildcommon

if __name__ == '__main__':
    root_dir = os.path.dirname(__file__)
    assembly_version_cs = 'src/AssemblyVersion.cs'
    assembly_version_cs_bak = 'src/AssemblyVersion.cs.bak'

    shutil.copy(os.environ['ASSEMBLY_KEY_PATH'],
                os.path.join(root_dir, 'key.snk'))

    version = buildcommon.get_version()

    print("##teamcity[buildNumber '" + version + "']")
