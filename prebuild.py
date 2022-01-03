import os
import shutil

if __name__ == '__main__':
    root_dir = os.path.dirname(__file__)
    shutil.copy(os.environ['ASSEMBLY_KEY_PATH'],
                os.path.join(root_dir, 'key.snk'))

    with open('version.txt', 'r') as f:
        semantic_version = f.read()

    build_counter = os.environ.get('BUILD_COUNTER') or '0'
    version = semantic_version + '.' + build_counter

    print("##teamcity[buildNumber '" + version + "']")
    print("##teamcity[setParameter name='system.Version' value='" + semantic_version + "']")
    print("##teamcity[setParameter name='system.DefineConstants' value='STRONG_NAME']")
    print("##teamcity[setParameter name='system.BuildCounter' value='" + build_counter + "']")
