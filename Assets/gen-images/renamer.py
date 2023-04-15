import os
import time


if __name__ == '__main__':
    image_name_prefix = "img-gen-"
    image_name_postfix = ".png"
    path_prefix = os.getcwd()

    for item in os.listdir(path_prefix):
        time.sleep(0.01)
        full_real_path = os.path.join(path_prefix, item)
        if full_real_path == __file__:
            continue
        stamp = str(time.time()).replace(".", "")
        full_new_path = f"{image_name_prefix}{str(stamp)}{image_name_postfix}"
        full_new_path = os.path.join(path_prefix, full_new_path)
        os.rename(full_real_path, full_new_path)