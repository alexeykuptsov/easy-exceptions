import os
import shutil

if __name__ == '__main__':
    root_dir = os.path.dirname(__file__)
    artifacts_dir = os.path.join(root_dir, 'artifacts')
    if os.path.isdir(artifacts_dir):
        shutil.rmtree(artifacts_dir)
    os.mkdir(artifacts_dir)

    with open('version.txt', 'r') as f:
        semantic_version = f.read()

    shutil.copy(
        os.path.join(root_dir, f'EasyExceptions.{semantic_version}.nupkg'),
        artifacts_dir + '/'
    )
